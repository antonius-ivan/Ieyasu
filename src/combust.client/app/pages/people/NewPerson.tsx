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
import { IPerson } from "./IPerson";

// Initial empty person
const initialPerson: IPerson = {
    id: 0,
    person_firstname: "",
    person_lastname: "",
    person_fullname: "",
    person_email: "",
    w_cellphone1: "",
    h_cellphone1: "",
    cre_dttm: undefined,
    cre_by: undefined,
    upd_dttm: undefined,
    upd_by: undefined,
};

export function NewPerson(): JSX.Element {
    const [person, setPerson] = useState<IPerson>(initialPerson);
    const [error, setError] = useState<string | null>(null);
    const [dialogOpen, setDialogOpen] = useState(false);
    const navigate = useNavigate();

    const handleSubmit = async (e: FormEvent) => {
        e.preventDefault();

        // Compute full name and audit fields
        const nowIso = new Date().toISOString();
        const payload = {
            ...person,
            person_fullname: `${person.person_firstname} ${person.person_lastname}`,
            cre_by: "admin",
            upd_by: "admin",
            cre_dttm: nowIso,
            upd_dttm: nowIso,
        };

        try {
            const response = await fetch("api/v1/tourney/people", {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify(payload),
            });

            if (!response.ok) {
                const detail = await response.text();
                throw new Error(`Status ${response.status}: ${detail}`);
            }

            navigate("/personlist");
        } catch (err: any) {
            setError(err.message);
            setDialogOpen(true);
        }
    };

    return (
        <form onSubmit={handleSubmit} style={{ maxWidth: 400, margin: "2rem auto" }}>
            <Field label="First Name" required style={{ marginBottom: "1rem" }}>
                <Input
                    value={person.person_firstname}
                    onChange={(e) =>
                        setPerson((p) => ({ ...p, person_firstname: e.target.value }))
                    }
                />
            </Field>

            <Field label="Last Name" required style={{ marginBottom: "1rem" }}>
                <Input
                    value={person.person_lastname}
                    onChange={(e) =>
                        setPerson((p) => ({ ...p, person_lastname: e.target.value }))
                    }
                />
            </Field>

            <Field label="Email" style={{ marginBottom: "1rem" }}>
                <Input
                    type="email"
                    value={person.person_email}
                    onChange={(e) =>
                        setPerson((p) => ({ ...p, person_email: e.target.value }))
                    }
                />
            </Field>

            <Field label="Work Cell" style={{ marginBottom: "1rem" }}>
                <Input
                    value={person.w_cellphone1}
                    onChange={(e) =>
                        setPerson((p) => ({ ...p, w_cellphone1: e.target.value }))
                    }
                />
            </Field>

            <Field label="Home Cell" style={{ marginBottom: "1.5rem" }}>
                <Input
                    value={person.h_cellphone1}
                    onChange={(e) =>
                        setPerson((p) => ({ ...p, h_cellphone1: e.target.value }))
                    }
                />
            </Field>

            <div style={{ display: "flex", gap: 8 }}>
                <Button type="submit">
                    Save
                </Button>
                <Button onClick={() => navigate("/personlist")}>
                    Cancel
                </Button>
            </div>

            <Dialog open={dialogOpen} onOpenChange={(_, data) => setDialogOpen(data.open)}>
                <DialogSurface>
                    <DialogTitle>
                        <Alert24Regular /> Error Saving Person
                    </DialogTitle>
                    <DialogBody>
                        <DialogContent>
                            We were unable to create the person.
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

export default NewPerson;
