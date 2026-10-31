
#!/usr/bin/env bash

set -Eeuo pipefail

APP_NAME="PeasantUprising"
DISPLAY_NAME="Peasant Uprising"

SOURCE_DIR="$(cd -- "$(dirname -- "${BASH_SOURCE[0]}")" && pwd)/../../"

DATA_HOME="${XDG_DATA_HOME:-$HOME/.local/share}"
CONFIG_HOME="${XDG_CONFIG_HOME:-$HOME/.config}"
BIN_DIR="$HOME/.local/bin"

INSTALL_DIR="$DATA_HOME/$APP_NAME"
DESKTOP_DIR="$DATA_HOME/applications"
DESKTOP_FILE="$DESKTOP_DIR/peasant-uprising.desktop"
LAUNCHER="$BIN_DIR/peasant-uprising"

echo "Installing $DISPLAY_NAME..."

# Check installation source
if [[ ! -d "$SOURCE_DIR" ]]; then
    echo "Error: Payload directory not found."
    exit 1
fi

if [[ ! -f "$SOURCE_DIR/PeasantUprising" ]]; then
    echo "Error: Game executable not found."
    exit 1
fi

# Create directories
mkdir -p "$INSTALL_DIR"
mkdir -p "$BIN_DIR"
mkdir -p "$DESKTOP_DIR"

# Copy game files
cp -a "$SOURCE_DIR/." "$INSTALL_DIR/"

# Set executable permission
chmod +x "$INSTALL_DIR/PeasantUprising"

# Create launcher
cat > "$LAUNCHER" <<EOF
#!/usr/bin/env bash
exec "$INSTALL_DIR/PeasantUprising" "\$@"
EOF

chmod +x "$LAUNCHER"

# Register desktop application
cat > "$DESKTOP_FILE" <<EOF
[Desktop Entry]
Type=Application
Name=$DISPLAY_NAME
Comment=Peasant Uprising
Exec=$LAUNCHER
Terminal=false
Categories=Game;StrategyGame;
EOF

chmod 644 "$DESKTOP_FILE"

echo
echo "$DISPLAY_NAME has been installed successfully!"
echo "Installation directory: $INSTALL_DIR"
echo "Launcher: $LAUNCHER"
echo
echo "You can now launch Peasant Uprising from your application menu."
