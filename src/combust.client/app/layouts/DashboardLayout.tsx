//\src\combust.client\app\layouts\DashboardLayout.tsx
import * as React from "react";
import {
    AppItem,
    Hamburger,
    NavCategory,
    NavCategoryItem,
    NavDivider,
    NavDrawer,
    NavDrawerBody,
    NavDrawerHeader,
    NavDrawerProps,
    NavItem,
    NavItemValue,
    NavSectionHeader,
    NavSubItem,
    NavSubItemGroup,
    OnNavItemSelectData,
} from "@fluentui/react-nav-preview";
import {
    Body1,
    Button,
    FluentProvider,
    Input,
    Label,
    Menu,
    MenuItem,
    MenuItemSwitch,
    MenuList,
    MenuPopover,
    MenuTrigger,
    Switch,
    Theme,
    Tooltip,
    makeStyles,
    tokens,
    useId,
    webLightTheme,
} from "@fluentui/react-components";
import {
    Board20Filled,
    Board20Regular,
    DataArea20Filled,
    DataArea20Regular,
    DocumentBulletListMultiple20Filled,
    DocumentBulletListMultiple20Regular,
    NotePin20Filled,
    NotePin20Regular,
    Person20Filled,
    Person20Regular,
    PersonSearch20Filled,
    PersonSearch20Regular,
    PreviewLink20Filled,
    PreviewLink20Regular,
    bundleIcon,
    PersonCircle32Regular,
    PaintBucketFilled,
    PaintBucketRegular,
    ArrowAutofitContent20Filled,
    PaintBucket20Filled,
    PaintBucket20Regular,
    ArrowAutofitContent20Regular,
    TableMoveBelow20Filled,
    TableMoveBelow20Regular,
    DrawerAdd20Filled,
    DrawerAdd20Regular,
    WhiteboardOff20Filled,
    WhiteboardOff20Regular,
    Search20Filled,
    Search20Regular,
} from "@fluentui/react-icons";
import { useAuth } from "../auths/AuthContext";
import TopBar from "../compolayouts/TopBar";
import { Outlet } from "react-router-dom";

const useClasses = makeStyles({
    root: {
        overflow: "hidden",
        display: "flex",
        height: "6400px",
    },
    navdrawer: {
        minWidth: "200px",
        "@media screen and (max-width: 768px)": {
            display: "none",        // hides drawer on narrow
        },
    },
    content: {
        flex: 1,
        padding: "16px",
        // push content to full width when drawer is hidden
        "@media (max-width: 768px)": {
            padding: "8px",
        },
    },
    field: {
        display: "flex",
        marginTop: "4px",
        marginLeft: "8px",
        flexDirection: "column",
        gridRowGap: tokens.spacingVerticalS,
    },
    contentBody: {
        flex: 1,
        padding: tokens.spacingVerticalS,
        overflow: "auto",
        display: "grid",
        gridTemplateColumns: "1fr",
        gap: tokens.spacingVerticalM,
        // Mobile first: small padding
        "@media screen and  (min-width: 768px)": {
            padding: tokens.spacingVerticalM,
            gridTemplateColumns: "repeat(2, 1fr)",
        },
        "@media screen and  (min-width: 1024px)": {
            padding: tokens.spacingVerticalL,
            gridTemplateColumns: "repeat(3, 1fr)",
        },
    },
});

const Person = bundleIcon(Person20Filled, Person20Regular);
const Dashboard = bundleIcon(Board20Filled, Board20Regular);
const PersonSearch = bundleIcon(PersonSearch20Filled, PersonSearch20Regular);
const PerformanceReviews = bundleIcon(
    PreviewLink20Filled,
    PreviewLink20Regular
);
const JobPostings = bundleIcon(NotePin20Filled, NotePin20Regular);
const Analytics = bundleIcon(DataArea20Filled, DataArea20Regular);
const Reports = bundleIcon(
    DocumentBulletListMultiple20Filled,
    DocumentBulletListMultiple20Regular
);
const PaintBucket = bundleIcon(PaintBucket20Filled, PaintBucket20Regular);
const ArrowAutofitContent = bundleIcon(ArrowAutofitContent20Filled, ArrowAutofitContent20Regular);
const TableMoveBelow = bundleIcon(TableMoveBelow20Filled, TableMoveBelow20Regular);
const DrawerAdd = bundleIcon(DrawerAdd20Filled, DrawerAdd20Regular); 
const WhiteboardOff = bundleIcon(WhiteboardOff20Filled, WhiteboardOff20Regular);
const Search = bundleIcon(Search20Filled, Search20Regular);

// A type that represents a navItemValue and its potential children.
// An empty children array indicates a Single top level NavItem.
// A hydrated children array indicates a NavCategoryItem with children.
type NavItemValueCombo = { parent: string; children: string[] };

// This is a list of navItemValues and their potential children
// Ite exactly matches the NavDrawer in the story below
// This is how a consumer might store them in their app
const navItemValueList: NavItemValueCombo[] = [
    { parent: "101", children: [] },
    { parent: "102", children: [] },
    { parent: "103", children: [] },
    { parent: "104", children: ["105", "106"] },
    { parent: "107", children: ["108", "109", "110", "111", "112", "300"] },
    { parent: "700", children: [] },
    { parent: "701", children: [] },
];

const allPages: SelectedPage[] = navItemValueList.flatMap((item): SelectedPage[] => {
    if (item.children.length === 0) {
        return [{ newSelectedCategory: undefined, newSelectedValue: item.parent }];
    }
    return item.children.map(child => ({
        newSelectedCategory: item.parent,
        newSelectedValue: child,
    }));
});

type SelectedPage = {
    newSelectedCategory: string | undefined;
    newSelectedValue: string;
};

const getRandomPage = (): SelectedPage => {
    const randomIndex = Math.floor(Math.random() * navItemValueList.length);
    const randomItem = navItemValueList[randomIndex];

    // there are no children, so we're selecting a top level item
    if (randomItem.children.length === 0) {
        return {
            newSelectedCategory: undefined,
            newSelectedValue: randomItem.parent,
        };
    } else {
        // there are children, so we're including a category and it's child as the selectedValue
        const randomChildIndex = Math.floor(
            Math.random() * randomItem.children.length
        );
        return {
            newSelectedCategory: randomItem.parent,
            newSelectedValue: randomItem.children[randomChildIndex],
        };
    }
};

export const DashboardLayout = (props: Partial<NavDrawerProps>) => {
    const classes = useClasses();

    const { isAuthenticated, user, login, logout } = useAuth();

    const [isOpen, setIsOpen] = React.useState(true);
    const [isTypeOverlay, setIsOverlayType] = React.useState(false);

    const [selectedCategoryValue, setSelectedCategoryValue] = React.useState<
        string | undefined
    >("101");
    const [selectedValue, setSelectedValue] = React.useState<string>("101");
    const [pageIndex, setPageIndex] = React.useState(0);
    const afterId = useId("content-after");

    const [searchValue, setSearchValue] = React.useState("");
    const [currentTheme, setCurrentTheme] = React.useState<Theme>(webLightTheme);

    const handleNavigationClick = () => {
        // advance to next index (wrap around)
        const next = (pageIndex + 1) % allPages.length;
        setPageIndex(next);

        const { newSelectedCategory, newSelectedValue } = allPages[next];
        setSelectedCategoryValue(newSelectedCategory ?? "");
        setSelectedValue(newSelectedValue);
    };

    const handleToggleOpen = () => {
        setIsOpen(!isOpen); // <-- flip the state
    };

    //// 3) log or swap every time it changes
    //React.useEffect(() => {
    //    console.log(`Viewport is ${isNarrow ? "≤ 768px" : "> 768px"}`);
    //}, [isNarrow]);

    return (
        <FluentProvider theme={currentTheme}>
            <div className={classes.root}>
                <NavDrawer
                    onNavItemSelect={handleNavigationClick}
                    tabbable={true} // enables keyboard tabbing
                    selectedValue={selectedValue}
                    type={isTypeOverlay ? "overlay" : "inline"} // convert boolean → string
                    open={isOpen}
                    className={classes.navdrawer}
                >
                    <NavDrawerHeader>
                        <Tooltip content="Close Navigation" relationship="label">
                            <Hamburger onClick={() => setIsOpen(!isOpen)} />
                        </Tooltip>
                    </NavDrawerHeader>
                    <NavDrawerBody>
                        <AppItem icon={<PersonCircle32Regular />} as="a">
                            Contoso HR
                        </AppItem>
                        <NavItem icon={<Dashboard />} value="101">
                            Dashboard
                        </NavItem>
                        <NavItem icon={<Search />} value="102">
                            Profile Search
                        </NavItem>
                        <NavItem icon={<PerformanceReviews />} value="103">
                            Performance Reviews
                        </NavItem>
                        <NavSectionHeader>Employee Management</NavSectionHeader>
                        <NavCategory value="104">
                            <NavCategoryItem icon={<JobPostings />}>
                                Job Postings
                            </NavCategoryItem>
                            <NavSubItemGroup>
                                <NavSubItem value="105">Openings</NavSubItem>
                                <NavSubItem value="106">Submissions</NavSubItem>
                            </NavSubItemGroup>
                        </NavCategory>
                        <NavSectionHeader>Tournament</NavSectionHeader>
                        <NavCategory value="107">
                            <NavCategoryItem icon={<Person />}>
                                All Tournament Module
                            </NavCategoryItem>
                            <NavSubItemGroup>
                                <NavSubItem href="/prizelist" value="108">
                                    Prize List
                                </NavSubItem>
                                <NavSubItem href="/personlist" value="109">
                                    Person List
                                </NavSubItem>
                                <NavSubItem href="/teamindex" value="110">
                                    Team Index
                                </NavSubItem>
                                <NavSubItem href="/sample05catalog" value="111">
                                    Sample05Catalog
                                </NavSubItem>
                                <NavSubItem href="/fuitrain02f" value="112">
                                    FuiTrain02FAdvStyleMediaQueries
                                </NavSubItem>
                                <NavSubItem href="/employeelist" value="300">
                                    Employee List
                                </NavSubItem>
                            </NavSubItemGroup>
                        </NavCategory>
                        <NavDivider />
                        <NavItem target="_blank" icon={<Analytics />} value="700">
                            Workforce Data
                        </NavItem>
                        <NavItem icon={<Reports />} value="701">
                            Reports
                        </NavItem>
                    </NavDrawerBody>
                </NavDrawer>
                <div className={classes.content}>
                    <TopBar
                        onTopToggleOpen={handleToggleOpen}
                        onTopNavigateClick={() => handleNavigationClick(null as any, { value: selectedValue })}
                        isTopTypeOverlay={isTypeOverlay}
                        setTopIsOverlayType={setIsOverlayType}
                        currentTopTheme={currentTheme}
                        onThemeTopChange={setCurrentTheme}
                    />
            
                    <div className={classes.contentBody}>
                        <Menu
                            checkedValues={isTypeOverlay
                                ? { "drawer-mode": ["drawer-mode"] }
                                : { "drawer-mode": [] }
                            }
                            onCheckedValueChange={(_, data) =>
                                setIsOverlayType(
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
                                        {isTypeOverlay ? "Overlay" : "Inline"}
                                    </MenuItemSwitch>
                                </MenuList>
                            </MenuPopover>
                        </Menu>
                        <Outlet />
                    </div>

                </div>
            </div>
        </FluentProvider>   
    );
};

export default DashboardLayout;