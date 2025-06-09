export interface IEmployee {
    /** Optional field for Employee ID */
    id?: number;

    /** Required field for first name */
    firstName: string;

    /** Required field for last name */
    lastName: string;

    /** Required field for email */
    email: string;

    /** Optional field for department */
    department?: string;

    /** Optional field for job title */
    jobTitle?: string;

    /** Optional field for hire date */
    hireDate?: Date;

    /** Required field for salary */
    salary: number;

    /** Required field for active status */
    isActive: boolean;

    /** Optional field for comments */
    comments?: string;

    /** Optional field for creation timestamp */
    createdAt?: Date;

    /** Optional field for creator's username */
    createdBy?: string;

    /** Optional field for last update timestamp */
    updatedAt?: Date;

    /** Optional field for updater's username */
    updatedBy?: string;
}
