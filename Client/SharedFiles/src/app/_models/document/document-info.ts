export class DocumentInfoDto {
    id: number;
    ownerId: number;
    owner: string;
    
    title: string;
    subject: string;

    createdOnUtc: string;
    updatedOnUtc: string;
}