export interface IPrize {
    id?: number;           // Optional field for Prize ID
    prizeNumber: number;   // Required field for Prize Number
    prizeName: string;     // Required field for Prize Name
    prizeAmount: number;   // Required field for Prize Amount
    prizePercentage: number; // Required field for Prize Percentage
    createdDate?: Date;    // Optional field for creation date
    createdBy?: string;    // Optional field for creator's name
    updatedDate?: Date;    // Optional field for last update date
    updatedBy?: string;    // Optional field for updater's name
}
