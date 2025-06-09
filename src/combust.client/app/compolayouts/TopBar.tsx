// src/combust.client/app/compolayouts/TopBar.tsx
import * as React from "react";
import {
    Menu,
    MenuItemSwitch,
    MenuList,
    MenuPopover,
    MenuTrigger,
    Button,
    makeStyles,
    tokens,
    MenuItem,
    Theme,
} from "@fluentui/react-components";
import { ArrowAutofitContent20Filled, ArrowAutofitContent20Regular, bundleIcon, DrawerAdd20Filled, DrawerAdd20Regular, PaintBucketFilled, PaintBucketRegular, Person20Filled, Person20Regular, TableMoveBelow20Filled, TableMoveBelow20Regular } from "@fluentui/react-icons";
import { unifiedThemes } from "../themebrands";
import { useAuth } from "../auths/AuthContext";

const useTopBarStyles = makeStyles({
    topBarContainer: {
        display: "flex",
        padding: tokens.spacingVerticalS,
        backgroundColor: tokens.colorBrandBackground2,
        justifyContent: "flex-end",
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
});

const ArrowAutofitContent = bundleIcon(ArrowAutofitContent20Filled, ArrowAutofitContent20Regular);
const TableMoveBelow = bundleIcon(TableMoveBelow20Filled, TableMoveBelow20Regular);
const DrawerAdd = bundleIcon(DrawerAdd20Filled, DrawerAdd20Regular);
const PaintBucket = bundleIcon(PaintBucketFilled, PaintBucketRegular);
const Person = bundleIcon(Person20Filled, Person20Regular);

interface TopBarProps {
    onTopNavigateClick: () => void;
    onTopToggleOpen: () => void;
    /** either "overlay" or "inline" */
    isTopTypeOverlay: boolean;
    /** fires with new mode */
    setTopIsOverlayType: (mode: boolean) => void;
    currentTopTheme: Theme;
    onThemeTopChange: (newTheme: Theme) => void;
}

export const TopBar: React.FC<TopBarProps> = ({
    onTopNavigateClick,
    onTopToggleOpen,
    isTopTypeOverlay,
    setTopIsOverlayType,
    currentTopTheme,
    onThemeTopChange,
}) => {
    const classes = useTopBarStyles();
    const { isAuthenticated, user, login, logout } = useAuth();

    return (
        <div className={classes.topBarContainer}>
            <Button appearance="subtle" onClick={onTopToggleOpen} icon={<ArrowAutofitContent />} />
            <Button appearance="subtle" onClick={onTopNavigateClick} icon={<TableMoveBelow />} />
            <Menu
                checkedValues={isTopTypeOverlay
                    ? { "drawer-mode": ["drawer-mode"] }
                    : { "drawer-mode": [] }
                }
                onCheckedValueChange={(_, data) =>
                    setTopIsOverlayType(
                        data.checkedItems.includes("drawer-mode")
                    )
                }
            >
                <MenuTrigger disableButtonEnhancement>
                    <Button appearance="subtle" icon={<DrawerAdd />} ></Button>
                </MenuTrigger>

                <MenuPopover>
                    <MenuList>
                        <MenuItemSwitch name="drawer-mode" value="drawer-mode">
                            {isTopTypeOverlay ? "Overlay" : "Inline"}
                        </MenuItemSwitch>
                    </MenuList>
                </MenuPopover>
            </Menu>

            {/* Right: Theme and user icons */}
            <div className={classes.iconGroup}>
                <Menu positioning={{ autoSize: true }}>
                    <MenuTrigger disableButtonEnhancement>
                        <Button icon={<PaintBucket />} />
                    </MenuTrigger>
                    <MenuPopover>
                        <MenuList>
                            {unifiedThemes.map((themeObj, idx) => {
                                // Highlight the currently active theme (optional)
                                const label =
                                    themeObj.brand.charAt(0).toUpperCase() +
                                    themeObj.brand.slice(1) +
                                    ` (${themeObj.mode})`;

                                return (
                                    <MenuItem
                                        key={idx}
                                        onClick={() => onThemeTopChange(themeObj.theme)}
                                        aria-checked={themeObj.theme === currentTopTheme}
                                    >
                                        {label}
                                    </MenuItem>
                                );
                            })}
                        </MenuList>
                    </MenuPopover>
                </Menu>

                <div>
                    {isAuthenticated && user ? (
                        <>
                            <span>Logged in as {user.email}</span>
                            <Button size="small" onClick={logout}>
                                Logout
                            </Button>
                        </>
                    ) : (
                        <Button onClick={login} icon={<Person />} />//Login
                    )}
                </div>
            </div>
        </div>
    );
};

export default TopBar;
