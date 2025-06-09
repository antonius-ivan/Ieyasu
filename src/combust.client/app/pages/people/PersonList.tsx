import React, { useEffect, useState } from "react";
import {
    DataGrid,
    DataGridBody,
    DataGridRow,
    DataGridHeader,
    DataGridHeaderCell,
    DataGridCell,
    TableCellLayout,
    TableColumnDefinition,
    createTableColumn,
    Button,
    Dialog,
    DialogTrigger,
    DialogTitle,
    DialogActions,
    DialogBody,
    DialogSurface,
    FluentProvider,
    webLightTheme,
} from "@fluentui/react-components";
import { EditRegular, DeleteRegular } from "@fluentui/react-icons";
import { useNavigate } from "react-router-dom";
import { IPerson } from "./IPerson";

export function PersonList(): JSX.Element {
    const [people, setPeople] = useState<IPerson[]>([]);
    const [loading, setLoading] = useState<boolean>(false);
    const [error, setError] = useState<string | null>(null);

    const [deleteDialogOpen, setDeleteDialogOpen] = useState(false);
    const [selectedPerson, setSelectedPerson] = useState<IPerson | null>(null);

    const navigate = useNavigate();

    const fetchPeople = async () => {
        setLoading(true);
        try {
            const response = await fetch("api/v1/tourney/people");
            if (!response.ok) throw new Error(`Error: ${response.status}`);
            const data: IPerson[] = await response.json();
            setPeople(data);
        } catch (err: any) {
            setError(err.message);
        } finally {
            setLoading(false);
        }
    };

    useEffect(() => {
        fetchPeople();
    }, []);

    const handleEdit = (p: IPerson) => {
        navigate(`/editperson/${p.id}`);
    };
    const handleDelete = (p: IPerson) => {
        setSelectedPerson(p);
        setDeleteDialogOpen(true);
    };
    const confirmDelete = async () => {
        if (!selectedPerson) return;
        const res = await fetch(`api/v1/tourney/people/${selectedPerson.id}`, {
            method: "DELETE",
        });
        if (res.ok) fetchPeople();
        setDeleteDialogOpen(false);
        setSelectedPerson(null);
    };

    const columns: TableColumnDefinition<IPerson>[] = [
        createTableColumn<IPerson>({
            columnId: "firstName",
            compare: (a, b) => a.person_firstname.localeCompare(b.person_firstname),
            renderHeaderCell: () => "First Name",
            renderCell: (item) => (
                <TableCellLayout>{item.person_firstname}</TableCellLayout>
            ),
        }),
        createTableColumn<IPerson>({
            columnId: "lastName",
            compare: (a, b) => a.person_lastname.localeCompare(b.person_lastname),
            renderHeaderCell: () => "Last Name",
            renderCell: (item) => (
                <TableCellLayout>{item.person_lastname}</TableCellLayout>
            ),
        }),
        createTableColumn<IPerson>({
            columnId: "fullName",
            compare: (a, b) => a.person_fullname.localeCompare(b.person_fullname),
            renderHeaderCell: () => "Full Name",
            renderCell: (item) => (
                <TableCellLayout>{item.person_fullname}</TableCellLayout>
            ),
        }),
        createTableColumn<IPerson>({
            columnId: "email",
            compare: (a, b) => (a.person_email ?? "").localeCompare(b.person_email ?? ""),
            renderHeaderCell: () => "Email",
            renderCell: (item) => <TableCellLayout>{item.person_email}</TableCellLayout>,
        }),
        createTableColumn<IPerson>({
            columnId: "workCell",
            compare: (a, b) => (a.w_cellphone1 ?? "").localeCompare(b.w_cellphone1 ?? ""),
            renderHeaderCell: () => "Work Cell",
            renderCell: (item) => <TableCellLayout>{item.w_cellphone1}</TableCellLayout>,
        }),
        createTableColumn<IPerson>({
            columnId: "homeCell",
            compare: (a, b) => (a.h_cellphone1 ?? "").localeCompare(b.h_cellphone1 ?? ""),
            renderHeaderCell: () => "Home Cell",
            renderCell: (item) => <TableCellLayout>{item.h_cellphone1}</TableCellLayout>,
        }),
        createTableColumn<IPerson>({
            columnId: "actions",
            renderHeaderCell: () => "Actions",
            renderCell: (item) => (
                <TableCellLayout style={{ display: "flex", gap: 8 }}>
                    <Button
                        appearance="primary"
                        icon={<EditRegular />}
                        onClick={() => handleEdit(item)}
                        title="Edit"
                        aria-label="Edit Person"
                    />
                    <Button
                        icon={<DeleteRegular />}
                        onClick={() => handleDelete(item)}
                        title="Delete"
                        aria-label="Delete Person"
                    />
                </TableCellLayout>
            ),
        }),
    ];

    if (loading) return <div>Loading…</div>;
    if (error) return <div style={{ color: "red" }}>Error: {error}</div>;

    return (
            <div>
                <div style={{ marginBottom: 16 }}>
                    <Button onClick={() => navigate("/newperson")}>
                        New Person
                    </Button>
                </div>

                <DataGrid
                    items={people}
                    columns={columns}
                    sortable
                    selectionMode="multiselect"
                    getRowId={(item) => item.id.toString()}
                    focusMode="composite"
                    style={{ minWidth: 800 }}
                >
                    <DataGridHeader>
                        <DataGridRow selectionCell={{ checkboxIndicator: { "aria-label": "Select all rows" } }}>
                            {({ renderHeaderCell }) => <DataGridHeaderCell>{renderHeaderCell()}</DataGridHeaderCell>}
                        </DataGridRow>
                    </DataGridHeader>

                    <DataGridBody<IPerson>>
                        {({ item, rowId }) => (
                            <DataGridRow<IPerson>
                                key={rowId}
                                selectionCell={{ checkboxIndicator: { "aria-label": "Select row" } }}
                            >
                                {({ renderCell }) => <DataGridCell>{renderCell(item)}</DataGridCell>}
                            </DataGridRow>
                        )}
                    </DataGridBody>
                </DataGrid>

                <Dialog
                    open={deleteDialogOpen}
                    onOpenChange={(e, data) => setDeleteDialogOpen(data.open)}
                >
                    <DialogSurface>
                        <DialogTitle>Confirm delete</DialogTitle>
                        <DialogBody>
                            Delete <strong>{selectedPerson?.person_fullname}</strong>?
                        </DialogBody>
                        <DialogActions>
                            <Button onClick={confirmDelete}>
                                Delete
                            </Button>
                            <DialogTrigger disableButtonEnhancement>
                                <Button>Cancel</Button>
                            </DialogTrigger>
                        </DialogActions>
                    </DialogSurface>
                </Dialog>
            </div>
    );
}

export default PersonList;
