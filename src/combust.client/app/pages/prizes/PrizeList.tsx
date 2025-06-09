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
import { IPrize } from "./IPrize";

export function PrizeList(): JSX.Element {
    //Hook and all the http fetch
    const [prizes, setPrizes] = useState<IPrize[]>([]);
    const [loading, setLoading] = useState<boolean>(false);
    const [error, setError] = useState<string | null>(null);

    const [deleteDialogOpen, setDeleteDialogOpen] = useState(false);
    const [selectedPrize, setSelectedPrize] = useState<IPrize | null>(null);

    const navigate = useNavigate();

    const fetchPrizes = async () => {
        setLoading(true);
        try {
            const response = await fetch("api/v1/tourney/prizes");
            if (!response.ok) {
                throw new Error(`Error: ${response.status}`);
            }
            const data: IPrize[] = await response.json();
            setPrizes(data);
        } catch (err: any) {
            setError(err.message);
        } finally {
            setLoading(false);
        }
    };

    //Fetch Use Effect for Render
    useEffect(() => {
        fetchPrizes();
    }, []);


    const handleEdit = (prize: IPrize) => {
        navigate(`/editprize/${prize.id}`);
    };

    const handleDelete = (prize: IPrize) => {
        setSelectedPrize(prize);
        setDeleteDialogOpen(true);
    };

    const confirmDelete = async () => {
        if (selectedPrize) {
            const response = await fetch(
                `api/v1/tourney/prizes/${selectedPrize.id}`,
                { method: "DELETE" }
            );
            if (response.ok) {
                await fetchPrizes();
            }
            setDeleteDialogOpen(false);
            setSelectedPrize(null);
        }
    };

    const columns: TableColumnDefinition<IPrize>[] = [
        createTableColumn<IPrize>({
            columnId: "prizeNumber",
            compare: (a, b) => a.prizeNumber - b.prizeNumber,
            renderHeaderCell: () => "Number",
            renderCell: item => <TableCellLayout>{item.prizeNumber}</TableCellLayout>,
        }),
        createTableColumn<IPrize>({
            columnId: "prizeName",
            compare: (a, b) => a.prizeName.localeCompare(b.prizeName),
            renderHeaderCell: () => "Name",
            renderCell: item => <TableCellLayout>{item.prizeName}</TableCellLayout>,
        }),
        createTableColumn<IPrize>({
            columnId: "prizeAmount",
            compare: (a, b) => a.prizeAmount - b.prizeAmount,
            renderHeaderCell: () => "Amount",
            renderCell: item => <TableCellLayout>{item.prizeAmount}</TableCellLayout>,
        }),
        createTableColumn<IPrize>({
            columnId: "prizePercentage",
            compare: (a, b) => a.prizePercentage - b.prizePercentage,
            renderHeaderCell: () => "Percentage",
            renderCell: item => <TableCellLayout>{item.prizePercentage}</TableCellLayout>,
        }),
        createTableColumn<IPrize>({
            columnId: "actions",
            renderHeaderCell: () => "Actions",
            renderCell: item => (
                <TableCellLayout style={{ display: "flex", gap: "8px" }}>
                    <Button
                        appearance="primary"
                        icon={<EditRegular />}
                        onClick={() => handleEdit(item)}
                        title="Edit"
                        aria-label="Edit Prize"
                    />
                    <Button
                        icon={<DeleteRegular />}
                        onClick={() => handleDelete(item)}
                        title="Delete"
                        aria-label="Delete Prize"
                    />
                </TableCellLayout>
            ),
        }),
    ];

    if (loading) return <div>Loading…</div>;
    if (error) return <div style={{ color: "red" }}>Error: {error}</div>;

    return (
            <div>
                <div style={{ marginBottom: "16px" }}>
                    <Button appearance="primary" onClick={() => navigate("/newprize")}>
                        New Prize
                    </Button>
                </div>

                <DataGrid
                    items={prizes}
                    columns={columns}
                    sortable
                    selectionMode="multiselect"
                    getRowId={item => item.id?.toString() ?? ''}
                    focusMode="composite"
                    style={{ minWidth: "650px" }}
                >
                    <DataGridHeader>
                        <DataGridRow
                            selectionCell={{ checkboxIndicator: { "aria-label": "Select all rows" } }}
                        >
                            {({ renderHeaderCell }) => (
                                <DataGridHeaderCell>{renderHeaderCell()}</DataGridHeaderCell>
                            )}
                        </DataGridRow>
                    </DataGridHeader>

                    <DataGridBody<IPrize>>
                        {({ item, rowId }) => (
                            <DataGridRow<IPrize>
                                key={rowId}
                                selectionCell={{ checkboxIndicator: { "aria-label": "Select row" } }}
                            >
                                {({ renderCell }) => <DataGridCell>{renderCell(item)}</DataGridCell>}
                            </DataGridRow>
                        )}
                    </DataGridBody>
                </DataGrid>

                <Dialog open={deleteDialogOpen} onOpenChange={(e, data) => setDeleteDialogOpen(data.open)}>
                    <DialogSurface>
                        <DialogTitle>Are you sure?</DialogTitle>
                        <DialogBody>
                            Do you want to delete the prize <strong>{selectedPrize?.prizeName}</strong>?
                        </DialogBody>
                        <DialogActions>
                            <Button appearance="primary" onClick={confirmDelete}>
                                Delete
                            </Button>
                            <DialogTrigger disableButtonEnhancement>
                                <Button>Close</Button>
                            </DialogTrigger>
                        </DialogActions>
                    </DialogSurface>
                </Dialog>
            </div>
    );
}

export default PrizeList;
