export class OtherClientDocumentIdsDto {
    DocumentId: number;
    ClientId: number;
    ActionType: InvitationActionType;
}

export enum InvitationActionType {
    View,
    Edit
}