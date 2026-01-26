using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Winslop.Properties;

namespace Winslop.Views
{
    /// <summary>
    /// AppsView encapsulates app scanning and uninstall logic for the Apps tab.
    /// </summary>
    public partial class AppsView : UserControl, IMainActions, ISearchable, IView
    {
        // Keeps the full unfiltered app list for search reset/filtering
        private string[] _allApps = Array.Empty<string>();

        private readonly AppManager _appManager;

        public AppsView(AppManager appManager)
        {
            _appManager = appManager ?? throw new ArgumentNullException(nameof(appManager));
            InitializeComponent();
        }

        /// <summary>
        /// Scans installed apps using patterns (external plugin list preferred, fallback to built-in list).
        /// Populates the CheckedListBox with detected apps.
        /// </summary>
        public async Task AnalyzeAsync()
        {
            checkedListBoxApps.Items.Clear();

            // Try loading patterns from CFEnhancer.txt (located in Plugins folder)
            var (bloatwarePatterns, whitelistPatterns, scanAll) = _appManager.LoadExternalBloatwarePatterns();

            if (bloatwarePatterns.Length == 0 && !scanAll)
            {
                // Fallback to internal resource if external file not found or empty and scanAll is not enabled
                bloatwarePatterns = Resources.PredefinedApps?
                    .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(s => s.Trim().ToLower())
                    .ToArray() ?? Array.Empty<string>();

                whitelistPatterns = Array.Empty<string>();
                Logger.Log("Using built-in bloatware list.", LogLevel.Info);
            }
            else
            {
                Logger.Log("🔎 Plugin ready: CFEnhancer (external bloatware list)", LogLevel.Info);
            }

            // Analyze installed apps based on patterns and whitelist, and optionally scan all
            var apps = await _appManager.AnalyzeAndLogAppsAsync(bloatwarePatterns, whitelistPatterns, scanAll);

            foreach (var app in apps)
                checkedListBoxApps.Items.Add(app.FullName);

            // Cache full list for search/filter (used by ISearchable)
            _allApps = apps.Select(a => a.FullName).ToArray();
        }

        /// <summary>
        /// Uninstalls all checked apps and removes them from the list after uninstall.
        /// </summary>
        public async Task FixAsync()
        {
            Logger.OutputBox?.Clear();

            // Fix selected Store apps
            var selectedApps = checkedListBoxApps.CheckedItems.Cast<string>().ToList();
            if (selectedApps.Count == 0)
                return;

            var removedApps = await _appManager.UninstallSelectedAppsAsync(selectedApps);

            // Update UI after uninstall
            foreach (var app in removedApps)
                checkedListBoxApps.Items.Remove(app);
        }

        /// <summary>
        /// Toggles all items in the app list (select all / select none).
        /// </summary>
        public void ToggleSelection()
        {
            bool shouldCheck = checkedListBoxApps.Items.Cast<object>()
                .Any(item => checkedListBoxApps.GetItemChecked(checkedListBoxApps.Items.IndexOf(item)) == false);

            for (int i = 0; i < checkedListBoxApps.Items.Count; i++)
                checkedListBoxApps.SetItemChecked(i, shouldCheck);
        }

        // ---------------- SearchInterface ----------------

        /// <summary>
        /// Rebuilds the CheckedListBox based on a search query.
        /// Empty query restores the full list.
        /// </summary>
        private void ApplyAppsFilter(string query)
        {
            // If nothing has been analyzed yet, there is nothing to filter
            if (_allApps == null || _allApps.Length == 0)
                return;

            string q = (query ?? string.Empty).Trim();

            // Preserve checked state by app name
            var checkedSet = checkedListBoxApps.CheckedItems
                .Cast<string>()
                .ToHashSet(StringComparer.OrdinalIgnoreCase);

            checkedListBoxApps.BeginUpdate();
            try
            {
                checkedListBoxApps.Items.Clear();

                var items = string.IsNullOrEmpty(q)
                    ? _allApps
                    : _allApps.Where(a => a.IndexOf(q, StringComparison.OrdinalIgnoreCase) >= 0).ToArray();

                foreach (var app in items)
                {
                    int idx = checkedListBoxApps.Items.Add(app);

                    // Restore checked state for items that were checked before filtering
                    if (checkedSet.Contains(app))
                        checkedListBoxApps.SetItemChecked(idx, true);
                }
            }
            finally
            {
                checkedListBoxApps.EndUpdate();
            }
        }

        public void ApplySearch(string query)
        {
            // Apply search to apps list (empty query = reset)
            ApplyAppsFilter(query);
        }

        public void RefreshView()
        {
            // Re-analyze apps when refreshing the view
            _ = AnalyzeAsync();
            Logger.Clear();
        }
    }
}
