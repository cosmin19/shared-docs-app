export class DocumentInfoDto {
    id: number;
    ownerId: number;
    owner: string;
    isOwnerDocument: boolean;
    
    title: string;
    subject: string;

    createdOnUtc: string;
    updatedOnUtc: string;
}