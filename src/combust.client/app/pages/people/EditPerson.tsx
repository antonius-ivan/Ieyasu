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
import { IPerson } from "./IPerson";

export function EditPerson(): JSX.Element {
    const { id } = useParams<{ id: string }>();
    const navigate = useNavigate();

    const [person, setPerson] = useState<IPerson>({
        id: undefined,
        person_firstname: "",
        person_lastname: "",
        person_fullname: "",
        person_email: "",
        w_cellphone1: "",
        h_cellphone1: "",
        cre_dttm: undefined,
        cre_by: "",
        upd_dttm: undefined,
        upd_by: "",
    });
    const [showSuccess, setShowSuccess] = useState(false);
    const [error, setError] = useState<string>("");
    const [dialogOpen, setDialogOpen] = useState(false);

    // Fetch the existing person when component mounts or `id` changes
    useEffect(() => {
        async function fetchPerson() {
            const resp = await fetch(`/api/v1/tourney/people/${id}`); // GET by id
            if (resp.ok) {
                const data: IPerson = await resp.json();
                setPerson(data);
            } else {
                setError(`Failed to load person #${id}`);
                setDialogOpen(true);
            }
        }
        if (id) {
            fetchPerson();
        }
    }, [id]);

    // Generic handler for input changes
    const handleInputChange =
        (field: keyof IPerson) =>
            (e: React.ChangeEvent<HTMLInputElement>) => {
                setPerson({ ...person, [field]: e.target.value });
            };

    // Submit the updated person via PUT (id in the URI)
    const handleSubmit = async (e: React.FormEvent<HTMLFormElement>) => {
        e.preventDefault();
        if (!id) return;

        // Compute full name & audit fields
        const updated = {
            ...person,
            id: Number(id),
            person_fullname: `${person.person_firstname} ${person.person_lastname}`,
            upd_by: "admin",
            upd_dttm: new Date().toISOString(),
        };

        const resp = await fetch(`/api/v1/tourney/people/${id}`, {
            method: "PUT",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify(updated),
        });

        if (resp.ok) {
            setShowSuccess(true);
            setTimeout(() => navigate("/personlist"), 1500);
        } else {
            const text = await resp.text();
            setError(text || "Unable to update the person.");
            setDialogOpen(true);
        }
    };

    return (
        <form onSubmit={handleSubmit} style={{ maxWidth: 400, margin: "2rem auto" }}>
            <Field label="First Name" required style={{ marginBottom: "1rem" }}>
                <Input
                    value={person.person_firstname}
                    onChange={handleInputChange("person_firstname")}
                />
            </Field>

            <Field label="Last Name" required style={{ marginBottom: "1rem" }}>
                <Input
                    value={person.person_lastname}
                    onChange={handleInputChange("person_lastname")}
                />
            </Field>

            <Field label="Email" style={{ marginBottom: "1rem" }}>
                <Input
                    type="email"
                    value={person.person_email}
                    onChange={handleInputChange("person_email")}
                />
            </Field>

            <Field label="Work Cell" style={{ marginBottom: "1rem" }}>
                <Input
                    value={person.w_cellphone1}
                    onChange={handleInputChange("w_cellphone1")}
                />
            </Field>

            <Field label="Home Cell" style={{ marginBottom: "1.5rem" }}>
                <Input
                    value={person.h_cellphone1}
                    onChange={handleInputChange("h_cellphone1")}
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

            {showSuccess && (
                <MessageBar style={{ marginTop: "1rem" }}>
                    Person updated successfully.
                </MessageBar>
            )}

            <Dialog open={dialogOpen} onOpenChange={(_, data) => setDialogOpen(data.open)}>
                <DialogSurface>
                    <DialogTitle>Error Saving Person</DialogTitle>
                    <DialogBody>
                        <DialogContent>
                            {error || "Unable to update the person."}
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

export default EditPerson;
