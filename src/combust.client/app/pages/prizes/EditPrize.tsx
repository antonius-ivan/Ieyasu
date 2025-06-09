import React, { useState, useEffect } from "react";
import { useParams, useNavigate } from "react-router-dom";
import {
    Field,
    Input,
    Button,
    Dialog,
    DialogSurface,
    DialogTitle,
    DialogBody,
    DialogContent,
    DialogActions,
    MessageBar,
} from "@fluentui/react-components";
import { IPrize } from "./IPrize";

export function EditPrize(): JSX.Element {
    const { id } = useParams<{ id: string }>();
    const navigate = useNavigate();

    const [prize, setPrize] = useState<IPrize>({
        id: undefined,
        prizeNumber: 0,
        prizeName: "",
        prizeAmount: 0,
        prizePercentage: 0,
        createdDate: undefined,
        createdBy: undefined,
        updatedDate: undefined,
        updatedBy: undefined,
    });
    const [showSuccess, setShowSuccess] = useState(false);
    const [error, setError] = useState<string>("");
    const [dialogOpen, setDialogOpen] = useState(false);

    // Fetch the existing prize when component mounts or `id` changes
    useEffect(() => {
        async function fetchPrize() {
            const resp = await fetch(`/api/v1/tourney/prizes/${id}`); // GET by id
            if (resp.ok) {
                const data: IPrize = await resp.json();
                setPrize(data);
            } else {
                setError(`Failed to load prize #${id}`);
                setDialogOpen(true);
            }
        }
        if (id) {
            fetchPrize();
        }
    }, [id]);

    // Generic handler for input changes
    const handleInputChange =
        (field: keyof IPrize) =>
            (e: React.ChangeEvent<HTMLInputElement>) => {
                let val: string | number = e.target.value;
                if (
                    field === "prizeNumber" ||
                    field === "prizeAmount" ||
                    field === "prizePercentage"
                ) {
                    val = parseFloat(val) || 0;
                }
                setPrize({ ...prize, [field]: val });
            };

    // Submit the updated prize via PUT (id in the URI)
    const handleSubmit = async (e: React.FormEvent<HTMLFormElement>) => {
        e.preventDefault();
        if (!id) return;

        // Ensure body.id matches the path id
        const body = { ...prize, id: parseInt(id, 10) };

        const resp = await fetch(`/api/v1/tourney/prizes/${id}`, {
            method: "PUT",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify(body),
        });

        if (resp.ok) {
            setShowSuccess(true);
            setTimeout(() => navigate("/prizelist"), 2000);
        } else {
            const text = await resp.text();
            setError(text || "Unable to update the prize.");
            setDialogOpen(true);
        }
    };

    return (
        <form onSubmit={handleSubmit} style={{ maxWidth: 400, margin: "2rem auto" }}>
            <Field label="Prize Number" required style={{ marginBottom: "1rem" }}>
                <Input
                    type="number"
                    placeholder="Enter prize number"
                    value={prize.prizeNumber}
                    onChange={handleInputChange("prizeNumber")}
                />
            </Field>

            <Field label="Prize Name" required style={{ marginBottom: "1rem" }}>
                <Input
                    placeholder="Enter prize name"
                    value={prize.prizeName}
                    onChange={handleInputChange("prizeName")}
                />
            </Field>

            <Field label="Amount" required style={{ marginBottom: "1rem" }}>
                <Input
                    type="number"
                    placeholder="0.00"
                    value={prize.prizeAmount}
                    onChange={handleInputChange("prizeAmount")}
                />
            </Field>

            <Field label="Percentage" required style={{ marginBottom: "1.5rem" }}>
                <Input
                    type="number"
                    placeholder="0.00"
                    value={prize.prizePercentage}
                    onChange={handleInputChange("prizePercentage")}
                />
            </Field>

            <div style={{ display: "flex", gap: 8 }}>
                <Button type="submit">
                    Save
                </Button>
                <Button onClick={() => navigate("/prizelist")}>
                    Cancel
                </Button>
            </div>

            {showSuccess && (
                <MessageBar style={{ marginTop: "1rem" }}>
                    Prize updated successfully.
                </MessageBar>
            )}

            <Dialog open={dialogOpen} onOpenChange={(_, data) => setDialogOpen(data.open)}>
                <DialogSurface>
                    <DialogTitle>Error Saving Prize</DialogTitle>
                    <DialogBody>
                        <DialogContent>
                            {error || "Unable to update the prize."}
                        </DialogContent>
                    </DialogBody>
                    <DialogActions>
                        <Button onClick={() => setDialogOpen(false)}>Close</Button>
                    </DialogActions>
                </DialogSurface>
            </Dialog>
        </form>
    );
}

export default EditPrize;
