
#!/usr/bin/env bash

set -Eeuo pipefail

APP_NAME="PeasantUprising"

DATA_HOME="${XDG_DATA_HOME:-$HOME/.local/share}"

INSTALL_DIR="$DATA_HOME/$APP_NAME"
LAUNCHER="$HOME/.local/bin/peasant-uprising"
DESKTOP_FILE="$DATA_HOME/applications/peasant-uprising.desktop"

echo "Uninstalling Peasant Uprising..."

# Check installation
if [[ ! -d "$INSTALL_DIR" ]]; then
    echo "Peasant Uprising is not installed."
    exit 1
fi

# Remove launcher
rm -f -- "$LAUNCHER"

# Remove desktop entry
rm -f -- "$DESKTOP_FILE"

# Remove game files
rm -rf -- "$INSTALL_DIR"

echo
echo "Peasant Uprising has been uninstalled."
echo "User configuration and save data outside the installation directory were preserved."
