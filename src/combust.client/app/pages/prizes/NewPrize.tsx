import React, { FormEvent, useState } from "react";
import { useNavigate } from "react-router-dom";
import {
    Button,
    Field,
    Input,
    Dialog,
    DialogSurface,
    DialogTitle,
    DialogBody,
    DialogActions,
    DialogContent,
} from "@fluentui/react-components";
import { Alert24Regular } from "@fluentui/react-icons";
import { IPrize } from "./IPrize";

// Initial empty prize
const initialPrize: IPrize = {
    id: 0,
    prizeNumber: 0,
    prizeName: "",
    prizeAmount: 0,
    prizePercentage: 0,
    createdDate: undefined,
    createdBy: undefined,
    updatedDate: undefined,
    updatedBy: undefined,
};

export function NewPrize(): JSX.Element {
    const [prize, setPrize] = useState<IPrize>(initialPrize);
    const [error, setError] = useState<string | null>(null);
    const [dialogOpen, setDialogOpen] = useState(false);
    const navigate = useNavigate();

    const handleSubmit = async (e: FormEvent) => {
        e.preventDefault();

        // Add audit fields expected by your API
        const nowIso = new Date().toISOString();
        const payload = {
            ...prize,
            createdBy: "admin",    // ≤5 chars if your backend enforces it
            updatedBy: "admin",
            createdDate: nowIso,
            updatedDate: nowIso,
        };
        fetch("api/v1/tourney/prizes");
        try {
            const response = await fetch(`api/v1/tourney/prizes`, {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify(payload),
            });

            if (!response.ok) {
                const detail = await response.text();
                throw new Error(`Status ${response.status}: ${detail}`);
            }

            // On success, go back to the prize list
            navigate("/prizelist");
        } catch (err: any) {
            setError(err.message);
            setDialogOpen(true);
        }
    };

    return (
        <form onSubmit={handleSubmit} style={{ maxWidth: 400, margin: "2rem auto" }}>
            <Field label="Prize Number" required style={{ marginBottom: "1rem" }}>
                <Input
                    type="number"
                    value={prize.prizeNumber}
                    onChange={(e) =>
                        setPrize((prev) => ({
                            ...prev,
                            prizeNumber: Number(e.target.value),
                        }))
                    }
                />
            </Field>

            <Field label="Prize Name" required style={{ marginBottom: "1rem" }}>
                <Input
                    placeholder="Enter prize name"
                    value={prize.prizeName}
                    onChange={(e) =>
                        setPrize((prev) => ({ ...prev, prizeName: e.target.value }))
                    }
                />
            </Field>

            <Field label="Prize Amount" required style={{ marginBottom: "1rem" }}>
                <Input
                    type="number"
                    placeholder="0.00"
                    value={prize.prizeAmount}
                    onChange={(e) =>
                        setPrize((prev) => ({
                            ...prev,
                            prizeAmount: Number(e.target.value),
                        }))
                    }
                />
            </Field>

            <Field label="Prize Percentage" required style={{ marginBottom: "1.5rem" }}>
                <Input
                    type="number"
                    placeholder="0.00"
                    value={prize.prizePercentage}
                    onChange={(e) =>
                        setPrize((prev) => ({
                            ...prev,
                            prizePercentage: Number(e.target.value),
                        }))
                    }
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

            <Dialog open={dialogOpen} onOpenChange={(_, data) => setDialogOpen(data.open)}>
                <DialogSurface>
                    <DialogTitle>
                        <Alert24Regular /> Error Saving Prize
                    </DialogTitle>
                    <DialogBody>
                        <DialogContent>
                            We were unable to create the prize.
                            {error && <div style={{ marginTop: 8 }}>Details: {error}</div>}
                        </DialogContent>
                    </DialogBody>
                    <DialogActions>
                        <Button onClick={() => setDialogOpen(false)}>
                            Close
                        </Button>
                    </DialogActions>
                </DialogSurface>
            </Dialog>
        </form>
    );
}

export default NewPrize;
