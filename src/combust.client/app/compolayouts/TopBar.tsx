//src\combust.client\app\compolayouts\TopBar.tsx
import * as React from "react";
import {
  Button,
  Input,
  Menu,
  MenuItem,
  MenuItemSwitch,
  MenuList,
  MenuPopover,
  MenuTrigger,
  Theme,
  makeStyles,
  tokens,
} from "@fluentui/react-components";
import {
  bundleIcon,
  PaintBucketFilled,
  PaintBucketRegular,
  Person20Filled,
  Person20Regular,
  Search20Regular,
  ArrowAutofitContent20Regular,
  TableMoveBelow20Regular,
  WhiteboardOff20Regular,
  DrawerAdd20Regular,
} from "@fluentui/react-icons";
import { unifiedThemes } from "../themebrands";

// Icon bundles
const Person = bundleIcon(Person20Filled, Person20Regular);
const PaintBucket = bundleIcon(PaintBucketFilled, PaintBucketRegular);

const useTopBarStyles = makeStyles({
  topBarContainer: {
    display: "flex",
    flexDirection: "column",
    padding: tokens.spacingVerticalXS,
    backgroundColor: tokens.colorBrandBackground2,
    gap: tokens.spacingVerticalXS,
    // Mobile first: stack vertically
    "@media (min-width: 480px)": {
      flexDirection: "row",
      padding: tokens.spacingVerticalS,
      alignItems: "center",
      justifyContent: "space-between",
      gap: tokens.spacingHorizontalS,
    },
    "@media (min-width: 768px)": {
      padding: tokens.spacingVerticalM,
      gap: tokens.spacingHorizontalM,
    },
  },
  iconGroup: {
    display: "flex",
    alignItems: "center",
    justifyContent: "center",
    gap: tokens.spacingHorizontalXXS,
    flexWrap: "wrap",
    // Mobile first: smaller gaps, center alignment
    "@media (min-width: 480px)": {
      justifyContent: "flex-start",
      gap: tokens.spacingHorizontalXS,
      flexWrap: "nowrap",
    },
    "@media (min-width: 768px)": {
      gap: tokens.spacingHorizontalS,
    },
  },
  search: {
    width: "100%",
    maxWidth: "none",
    order: -1, // Put search first on mobile
    // Mobile first: full width
    "@media (min-width: 480px)": {
      order: 0,
      flexGrow: 1,
      maxWidth: "300px",
      marginLeft: tokens.spacingHorizontalS,
      marginRight: tokens.spacingHorizontalS,
    },
    "@media (min-width: 768px)": {
      maxWidth: "400px",
      marginLeft: tokens.spacingHorizontalM,
      marginRight: tokens.spacingHorizontalM,
    },
    "@media (min-width: 1024px)": {
      maxWidth: "500px",
    },
  },
  mobileControls: {
    display: "flex",
    justifyContent: "space-between",
    alignItems: "center",
    width: "100%",
    "@media (min-width: 480px)": {
      width: "auto",
      justifyContent: "flex-start",
    },
  },
});

interface TopBarProps {
  onNavigateClick: () => void;
  onToggleOpen: () => void;
  isSwitchOverlay: boolean;
  onSwitchChange: (checked: boolean) => void;
  onSearchChange?: (value: string) => void;
  searchValue?: string;
  currentTheme: Theme;
  drawerMode: boolean;
  setDrawerMode: (mode: boolean) => void;
  onThemeChange: (newTheme: Theme) => void;
}

export const TopBar: React.FC<TopBarProps> = ({
    onNavigateClick,
    onToggleOpen,
    drawerMode,
    setDrawerMode,
    onSearchChange,
    searchValue = "",
    currentTheme,
    onThemeChange,
}) => {
  const classes = useTopBarStyles();

  return (
    <div className={classes.topBarContainer}>
      {/* Search input - shows first on mobile */}
      <Input
        className={classes.search}
        contentAfter={<Search20Regular />}
        placeholder="Search..."
        value={searchValue}
        onChange={(_, data) => onSearchChange && onSearchChange(data.value)}
      />

      {/* Mobile: Controls split between left and right */}
      <div className={classes.mobileControls}>
        {/* Left: Control icons */}
        <div className={classes.iconGroup}>
          <Button
            appearance="subtle"
            onClick={onToggleOpen}
            icon={<ArrowAutofitContent20Regular />}
          />
          <Button
            appearance="subtle"
            onClick={onNavigateClick}
            icon={<TableMoveBelow20Regular />}
          />
          <Button appearance="subtle" icon={<WhiteboardOff20Regular />} />
            <Menu>
                <MenuTrigger disableButtonEnhancement>
                    <Button appearance="subtle" icon={<DrawerAdd20Regular />} />
                </MenuTrigger>
                <MenuPopover>
                    <MenuList>
                        <MenuItemSwitch
                            name="drawer-mode"
                            value="drawer-mode"
                            checked={drawerMode}
                            onChange={(_, data) => setDrawerMode(data.checked)}
                        >
                            {drawerMode ? "Overlay" : "Inline"}
                        </MenuItemSwitch>
                    </MenuList>
                </MenuPopover>
            </Menu>
        </div>

        {/* Right: Theme and user icons */}
        <div className={classes.iconGroup}>
          <Menu>
            <MenuTrigger disableButtonEnhancement>
              <Button appearance="subtle" icon={<PaintBucket />} />
            </MenuTrigger>
            <MenuPopover>
              <MenuList>
                {unifiedThemes.map((themeObj, idx) => {
                  const label =
                    themeObj.brand.charAt(0).toUpperCase() +
                    themeObj.brand.slice(1) +
                    ` (${themeObj.mode})`;

                  return (
                    <MenuItem
                      key={idx}
                      onClick={() => onThemeChange(themeObj.theme)}
                      aria-checked={themeObj.theme === currentTheme}
                    >
                      {label}
                    </MenuItem>
                  );
                })}
              </MenuList>
            </MenuPopover>
          </Menu>
          <Button appearance="subtle" icon={<Person />} />
        </div>
      </div>
    </div>
  );
};

export default TopBar;
