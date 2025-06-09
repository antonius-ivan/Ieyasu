export interface IPerson {
    id?: number;                // Optional field for Person ID
    person_firstname: string;   // Required: first name
    person_lastname: string;    // Required: last name
    person_fullname: string;    // Required: full name (you may compute this client-side or trust the API)
    person_email?: string;      // Optional: email
    w_cellphone1?: string;      // Optional: work cellphone
    h_cellphone1?: string;      // Optional: home cellphone
    cre_dttm?: string;          // Optional: creation timestamp (ISO string)
    cre_by?: string;            // Optional: creator’s user/code
    upd_dttm?: string;          // Optional: last-update timestamp (ISO string)
    upd_by?: string;            // Optional: updater’s user/code
}
