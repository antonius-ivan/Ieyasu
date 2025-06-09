import * as React from "react";
import {
    FolderRegular,
    EditRegular,
    OpenRegular,
    DocumentRegular,
    DocumentPdfRegular,
    VideoRegular,
    DeleteRegular,
} from "@fluentui/react-icons";
import {
    PresenceBadgeStatus,
    Avatar,
    DataGridBody,
    DataGridRow,
    DataGrid,
    DataGridHeader,
    DataGridHeaderCell,
    DataGridCell,
    TableCellLayout,
    TableColumnDefinition,
    createTableColumn,
    Button,
    TableColumnId,
    DataGridCellFocusMode,
} from "@fluentui/react-components";
import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import { IEmployee } from "./IEmployee";

type FileCell = {
    label: string;
    icon: JSX.Element;
};

type LastUpdatedCell = {
    label: string;
    timestamp: number;
};

type AuthorCell = {
    label: string;
    status: PresenceBadgeStatus;
};

type Item = {
    file: FileCell;
    author: AuthorCell;
    lastUpdated: LastUpdatedCell;
};

const items: Item[] = [
    { file: { label: "Meeting notes", icon: <DocumentRegular /> }, author: { label: "Max Mustermann", status: "available" }, lastUpdated: { label: "7h ago", timestamp: 1 } },
    { file: { label: "Thursday presentation", icon: <FolderRegular /> }, author: { label: "Erika Mustermann", status: "busy" }, lastUpdated: { label: "Yesterday at 1:45 PM", timestamp: 2 } },
    { file: { label: "Training recording", icon: <VideoRegular /> }, author: { label: "John Doe", status: "away" }, lastUpdated: { label: "Yesterday at 1:45 PM", timestamp: 2 } },
    { file: { label: "Purchase order", icon: <DocumentPdfRegular /> }, author: { label: "Jane Doe", status: "offline" }, lastUpdated: { label: "Tue at 9:30 AM", timestamp: 3 } },
];


function getCellFocusMode(columnId: TableColumnId): DataGridCellFocusMode {
    switch (columnId) {
        case "singleAction":
            return "none";
        case "actions":
            return "group";
        default:
            return "cell";
    }
}

//2025-04-28 19:58 Body FUnction -> exportfunction
//2025-04-28 20:00 Body FUnction -> JSX Element
export function EmployeeList(): JSX.Element {
    const [employees, setEmployees] = useState<IEmployee[]>([]);
    const [loading, setLoading] = useState<boolean>(false);
    const [error, setError] = useState<string | null>(null);

    const navigate = useNavigate();

    const fetchEmployees = async () => {
        setLoading(true);
        try {
            const response = await fetch("/api/v1/tourney/employees");
            if (!response.ok) {
                throw new Error(`Error: ${response.status}`);
            }
            const data: IEmployee[] = await response.json();
            setEmployees(data);
        } catch (err: any) {
            setError(err.message);
        } finally {
            setLoading(false);
        }
    };

    useEffect(() => {
        fetchEmployees();
        console.log(employees);
    }, []);

    const handleEdit = (employee: IEmployee) => {
        navigate(`/editemployee/${employee.id}`);
    };


    const columns: TableColumnDefinition<IEmployee>[] = [
        createTableColumn<IEmployee>({
            columnId: "id",
            compare: (a, b) => (a.id ?? 0) - (b.id ?? 0),
            renderHeaderCell: () => "ID",
            renderCell: (item) => (
                <TableCellLayout>{item.id}</TableCellLayout>
            ),
        }),
        createTableColumn<IEmployee>({
            columnId: "firstName",
            compare: (a, b) => a.firstName.localeCompare(b.firstName),
            renderHeaderCell: () => "First Name",
            renderCell: (item) => (
                <TableCellLayout>{item.firstName}</TableCellLayout>
            ),
        }),
        createTableColumn<IEmployee>({
            columnId: "lastName",
            compare: (a, b) => a.lastName.localeCompare(b.lastName),
            renderHeaderCell: () => "Last Name",
            renderCell: (item) => (
                <TableCellLayout>{item.lastName}</TableCellLayout>
            ),
        }),
        createTableColumn<IEmployee>({
            columnId: "email",
            compare: (a, b) => a.email.localeCompare(b.email),
            renderHeaderCell: () => "Email",
            renderCell: (item) => (
                <TableCellLayout>{item.email}</TableCellLayout>
            ),
        }),
        createTableColumn<IEmployee>({
            columnId: "department",
            compare: (a, b) => (a.department ?? "").localeCompare(b.department ?? ""),
            renderHeaderCell: () => "Department",
            renderCell: (item) => (
                <TableCellLayout>{item.department ?? "N/A"}</TableCellLayout>
            ),
        }),
        createTableColumn<IEmployee>({
            columnId: "salary",
            compare: (a, b) => (a.salary ?? 0) - (b.salary ?? 0),
            renderHeaderCell: () => "Salary",
            renderCell: (item) => (
                <TableCellLayout>
                    {item.salary !== undefined && item.salary !== null
                        ? item.salary.toLocaleString("en-US", { style: "currency", currency: "USD" })
                        : "N/A"}
                </TableCellLayout>
            ),
        }),
        createTableColumn<IEmployee>({
            columnId: "isActive",
            compare: (a, b) => Number(a.isActive) - Number(b.isActive),
            renderHeaderCell: () => "Active",
            renderCell: (item) => (
                <TableCellLayout>{item.isActive ? "Yes" : "No"}</TableCellLayout>
            ),
        }),
        createTableColumn<IEmployee>({
            columnId: "actions",
            renderHeaderCell: () => "Actions",
            renderCell: (item) => (
                <TableCellLayout style={{ display: "flex", gap: "8px" }}>
                    <Button
                        appearance="primary"
                        icon={<EditRegular />}
                        onClick={() => handleEdit(item)}
                        title="Edit"
                        aria-label="Edit Employee"
                    />
                    <Button
                        icon={<DeleteRegular />}
                        onClick={() => console.log("Delete", item.id)}
                        title="Delete"
                        aria-label="Delete Employee"
                    />
                </TableCellLayout>
            ),
        }),
    ];

    return (
        <div>
            <Button
                appearance="primary" onClick={() => navigate("/newprize")}>
                New Employee
            </Button>
            <DataGrid
                items={employees}
                columns={columns}
                sortable
                selectionMode="multiselect"
                getRowId={item => item.id?.toString() ?? ''}
                onSelectionChange={(e, data) => console.log(data)}
                style={{ minWidth: "550px" }}
            >
                <DataGridHeader>
                    <DataGridRow
                        selectionCell={{
                            checkboxIndicator: { "aria-label": "Select all rows" },
                        }}
                    >
                        {({ renderHeaderCell }) => (
                            <DataGridHeaderCell>{renderHeaderCell()}</DataGridHeaderCell>
                        )}
                    </DataGridRow>
                </DataGridHeader>
                <DataGridBody<IEmployee>>
                    {({ item, rowId }) => (
                        <DataGridRow<IEmployee>
                            key={rowId}
                            selectionCell={{
                                checkboxIndicator: { "aria-label": "Select row" },
                            }}
                        >
                            {({ renderCell, columnId }) => (
                                <DataGridCell focusMode={getCellFocusMode(columnId)}>
                                    {renderCell(item)}
                                </DataGridCell>
                            )}
                        </DataGridRow>
                    )}
                </DataGridBody>
            </DataGrid>
        </div>
    );
}



export default EmployeeList;
