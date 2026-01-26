using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Winslop.Extensions
{
    public partial class ExtensionsView : UserControl, ISearchable, IView
    {
        private ExtensionsCategory _category; // The category this view represents
        private readonly List<ExtensionsDefinition> _allTools = new List<ExtensionsDefinition>();

        // Caches UI controls for each tool so they are not recreated every time.
        private readonly Dictionary<ExtensionsDefinition, ExtensionsItemControl> _controlCache
            = new Dictionary<ExtensionsDefinition, ExtensionsItemControl>();

        // Pending tool selection (if any) to apply after loading
        private string _pendingSelectTool = null;

        // Overloaded constructor with category filter
        public ExtensionsView(ExtensionsCategory category = ExtensionsCategory.All)
        {
            InitializeComponent();
            _category = category;
            comboFilter.Items.AddRange(new object[] { "All", "Tool", "Pre", "Mid", "Post" });
            //comboFilter.SelectedItem = _category.ToString();
            // Prevent the SelectedIndexChanged event from firing during initial setup,
            // because changing SelectedIndex will otherwise trigger LoadTools() too early.
            comboFilter.SelectedIndexChanged -= comboFilter_SelectedIndexChanged;
            comboFilter.SelectedIndex = 0; // Set default filter UI state without applying filtering logic yet
            comboFilter.SelectedIndexChanged += comboFilter_SelectedIndexChanged;

            LoadTools();
        }

        private async void LoadTools()
        {
            lblStatus.Visible = true;
            // Prevent flicker during bulk UI update
            flowLayoutPanelTools.SuspendLayout();
            flowLayoutPanelTools.Controls.Clear();
            _controlCache.Clear();
            _allTools.Clear();

            // Define the scripts directory relative to the executable path
            string scriptDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "scripts");

            // If the directory does not exist, create it
            if (!Directory.Exists(scriptDirectory))
            {
                Directory.CreateDirectory(scriptDirectory);
                flowLayoutPanelTools.ResumeLayout();
                return;
            }

            // Get all .ps1 files in the folder (async to avoid UI freeze)
            string[] scriptFiles = await Task.Run(() => Directory.GetFiles(scriptDirectory, "*.ps1"));

            // Parse metadata and construct tool definitions in background
            var loadedTools = await Task.Run(() =>
            {
                var list = new List<ExtensionsDefinition>();

                // Loop through each script and create a tool item
                foreach (var scriptPath in scriptFiles)
                {
                    string fileName = Path.GetFileNameWithoutExtension(scriptPath); // Use file name as title
                    string icon = PickIconForScript(fileName);                      // Choose an icon based on the name

                    // Read metadata (description, options, category, etc.)
                    var meta = ReadMetadataFromScript(scriptPath);

                    // Skip tools not matching the current category
                    if (_category != ExtensionsCategory.All && meta.category != _category)
                        continue;

                    // Create tool definition
                    var tool = new ExtensionsDefinition(fileName, meta.description, icon, scriptPath);
                    tool.Options.AddRange(meta.options);
                    tool.UseConsole = meta.useConsole;
                    tool.UseLog = meta.useLog;
                    tool.SupportsInput = meta.inputEnabled;
                    tool.InputPlaceholder = meta.inputPh;
                    tool.PoweredByText = meta.poweredByText;
                    tool.PoweredByUrl = meta.poweredByUrl;
                    list.Add(tool);                // Save for search/filter
                }

                return list;
            });

            // Create UI controls now, but only once (no recreation during filtering)
            foreach (var tool in loadedTools)
            {
                var control = new ExtensionsItemControl(tool);
                _controlCache[tool] = control; // Cache it so filtering is instant
                flowLayoutPanelTools.Controls.Add(control);
                _allTools.Add(tool);
            }

            lblStatus.Visible = false;
            flowLayoutPanelTools.ResumeLayout();
        }

        /// <summary>
        /// Reads metadata from a script header, such as description, category, and options.
        /// Example in .ps1:
        /// # Description: Cleans pre-installed apps
        /// # Category: Post
        /// # Options: Light;Full
        /// # Host: log
        /// </summary>
        // Parses script header metadata (first ~15 lines) and returns all fields.
        private (string description,
                 List<string> options,
                 ExtensionsCategory category,
                 bool useConsole,
                 bool useLog,
                 bool inputEnabled,
                 string inputPh,
                 string poweredByText,
                 string poweredByUrl)
            ReadMetadataFromScript(string scriptPath)
        {
            string description = "No description available.";
            var options = new List<string>();
            ExtensionsCategory category = ExtensionsCategory.All;
            bool useConsole = false;
            bool useLog = false;
            bool inputEnabled = false;
            string inputPh = string.Empty;
            string poweredByText = string.Empty;
            string poweredByUrl = string.Empty;

            try
            {
                // Only scan the first lines for metadata.
                // Extensions put their headers (# Description, # Host, # Options) at the top,
                // so we dont waste time parsing a huge script body.
                var lines = File.ReadLines(scriptPath).Take(15);

                foreach (var line in lines)
                {
                    if (string.IsNullOrWhiteSpace(line))
                        continue;

                    if (line.StartsWith("# Description:", StringComparison.OrdinalIgnoreCase))
                    {
                        description = line.Substring(14).Trim();
                    }
                    else if (line.StartsWith("# Category:", StringComparison.OrdinalIgnoreCase))
                    {
                        string raw = line.Substring(11).Trim().ToLowerInvariant();
                        if (raw == "pre") category = ExtensionsCategory.Pre;
                        else if (raw == "mid") category = ExtensionsCategory.Mid;
                        else if (raw == "tool") category = ExtensionsCategory.Tool;
                        else if (raw == "post") category = ExtensionsCategory.Post;
                        else category = ExtensionsCategory.All;
                    }
                    else if (line.StartsWith("# Options:", StringComparison.OrdinalIgnoreCase))
                    {
                        // Parse dropdown options
                        options = line.Substring(10)
                            .Split(';')
                            .Select(x => x.Trim())
                            .Where(x => !string.IsNullOrEmpty(x)) // ignore empty entries
                            .ToList();
                    }
                    else if (line.StartsWith("# Host:", StringComparison.OrdinalIgnoreCase))
                    {
                        // Parse host type
                        var raw = line.Substring(7).Trim().ToLowerInvariant();
                        if (raw == "console") // standard console window
                            useConsole = true;
                        else if (raw == "log") // custom log viewer
                            useLog = true;
                        // "embedded"/"silent" == both false
                    }
                    else if (line.StartsWith("# Input:", StringComparison.OrdinalIgnoreCase))
                    {
                        var raw = line.Substring(8).Trim().ToLowerInvariant();
                        inputEnabled = (raw == "true" || raw == "yes" || raw == "1");
                    }
                    else if (line.StartsWith("# InputPlaceholder:", StringComparison.OrdinalIgnoreCase))
                    {
                        inputPh = line.Substring(19).Trim();
                    }
                    // PoweredBy metadata (optional)
                    else if (line.StartsWith("# PoweredBy:", StringComparison.OrdinalIgnoreCase))
                        poweredByText = line.Substring(12).Trim();   // 11 chars + 1 for :
                    else if (line.StartsWith("# PoweredUrl:", StringComparison.OrdinalIgnoreCase))
                        poweredByUrl = line.Substring(13).Trim();    // 12 chars + 1 for :

                    // Use the first regular comment as description (if none yet)
                    else if (line.StartsWith("#"))
                    {
                        if (description == "No description available.")
                            description = line.TrimStart('#').Trim();
                    }
                }
            }
            catch
            {
                // Ignore errors and keep defaults
            }
            return (description, options, category, useConsole, useLog, inputEnabled, inputPh, poweredByText, poweredByUrl);
        }

        private string PickIconForScript(string name)
        {
            name = name.ToLower();

            // Basic keyword-based icon picking using Segoe MDL2 Assets
            if (name.Contains("debloat")) return "\uE74D";      // Trash icon (cleanup)
            if (name.Contains("network")) return "\uE701";      // Network icon (network tools)
            if (name.Contains("explorer")) return "\uE8B7";     // File Explorer icon (file management)
            if (name.Contains("update")) return "\uE895";       // Update icon (system updates)
            if (name.Contains("context")) return "\uE8A5";      // Menu icon (context menu tweaks)

            // Additional general categories
            if (name.Contains("backup")) return "\uE8C7";       // Save icon (backup tools)
            if (name.Contains("security")) return "\uE72E";     // Shield icon (security tools)
            if (name.Contains("performance")) return "\uE7B8";  // Speedometer icon (performance)
            if (name.Contains("privacy")) return "\uF552";      // Privacy icon (privacy settings)
            if (name.Contains("app")) return "\uED35";          // App icon (application management)
            if (name.Contains("setup")) return "\uE8FB";        // Install icon (installers)

            return "\uE7C5"; // Wrench icon (default for uncategorized tools)
        }

        public void RefreshView()
        {
            flowLayoutPanelTools.Controls.Clear();
            LoadTools();

            Logger.Clear(); // Clear log output when refreshing the view
        }

        private void comboFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (comboFilter.SelectedItem.ToString())
            {
                case "Tool": _category = ExtensionsCategory.Tool; break;
                case "Pre": _category = ExtensionsCategory.Pre; break;
                case "Mid": _category = ExtensionsCategory.Mid; break;
                case "Post": _category = ExtensionsCategory.Post; break;
                default: _category = ExtensionsCategory.All; break;
            }

            LoadTools(); // re-render based on new category
        }

        // ---------------- SearchInterface ----------------
        private void DisplayFilteredTools(string filter)
        {
            flowLayoutPanelTools.SuspendLayout();
            flowLayoutPanelTools.Controls.Clear();

            string f = filter?.ToLowerInvariant() ?? "";

            foreach (var kv in _controlCache)
            {
                var tool = kv.Key;
                var control = kv.Value;

                bool show = string.IsNullOrWhiteSpace(f)
                            || tool.Title.ToLower().Contains(f)
                            || tool.Description.ToLower().Contains(f);

                if (show)
                    flowLayoutPanelTools.Controls.Add(control);
            }

            flowLayoutPanelTools.ResumeLayout();
        }

        public void ApplySearch(string query)
        {
            DisplayFilteredTools(query);
        }
    }
}