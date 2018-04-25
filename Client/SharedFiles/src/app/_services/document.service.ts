import { Injectable } from "@angular/core";
import { BaseService } from "./base.service";
import { HttpClient, HttpParams } from "@angular/common/http";
import { AppConfig } from "../app.config";
import { MessageDto } from "../_models/message";
import { Observable } from "rxjs/Observable";
import { DocumentMiniInfo } from "../_models/document/document-mini-info";
import { DocumentInfoDto } from "../_models/document/document-info";
import { NotificationDto } from "../_models/notification";
import { OtherClientDocumentIdsDto } from "../_models/other-client-document";
import { ClientDto } from "../_models/client";

@Injectable()
export class DocumentService extends BaseService {

    baseUrl: string = '';

    constructor(
        private http: HttpClient,
        private _appConfig: AppConfig
    ) {
        super();
        this.baseUrl = _appConfig.baseUrl;
    }

    addNewDocument(document: any): Observable<MessageDto> {
        let bodyData = JSON.stringify(document);
        const headers = this.jwt_auth_content_type_json().headers;

        return this.http.put<MessageDto>(this.baseUrl + 'api/document/', bodyData, { headers: headers });
    }

    editDocument(document: any): Observable<MessageDto> {
        let bodyData = JSON.stringify(document);
        const headers = this.jwt_auth_content_type_json().headers;

        return this.http.post<MessageDto>(this.baseUrl + 'api/document/edit', bodyData, { headers: headers });
    }

    getAllDocumentsForUser(): Observable<DocumentMiniInfo[]> {
        const headers = this.jwt_auth_content_type_json().headers;

        return this.http.get<DocumentMiniInfo[]>(this.baseUrl + 'api/document/', { headers: headers });
    }

    getDocumentById(id: number): Observable<DocumentInfoDto> {
        const headers = this.jwt_auth_content_type_json().headers;
        const params = new HttpParams().set('id', id.toString());

        return this.http.get<DocumentInfoDto>(this.baseUrl + 'api/document/info', { headers: headers, params: params });
    }

    deleteDocument(id: number): Observable<MessageDto> {
        const headers = this.jwt_auth_content_type_json().headers;
        const params = new HttpParams().set('id', id.toString());

        return this.http.post<MessageDto>(this.baseUrl + 'api/document/delete', {}, { headers: headers, params: params });
    }

    viewDocument(id: number): Observable<MessageDto> {
        const headers = this.jwt_auth_content_type_json().headers;
        const params = new HttpParams().set('id', id.toString());

        return this.http.get<MessageDto>(this.baseUrl + 'api/document/', { headers: headers, params: params });
    }

    listDocumentsView(): Observable<DocumentMiniInfo[]> {
        const headers = this.jwt_auth_content_type_json().headers;

        return this.http.get<DocumentMiniInfo[]>(this.baseUrl + 'api/document/listdocumentsview', { headers: headers });
    }

    listDocumentsEdit(): Observable<DocumentMiniInfo[]> {
        const headers = this.jwt_auth_content_type_json().headers;

        return this.http.get<DocumentMiniInfo[]>(this.baseUrl + 'api/document/listdocumentsedit', { headers: headers });
    }

    getNotifications(): Observable<NotificationDto[]> {
        const headers = this.jwt_auth_content_type_json().headers;

        return this.http.get<NotificationDto[]>(this.baseUrl + 'api/document/notifications', { headers: headers });
    }

    inviteClient(model: OtherClientDocumentIdsDto): Observable<MessageDto> {
        let bodyData = JSON.stringify(model);
        const headers = this.jwt_auth_content_type_json().headers;

        return this.http.post<MessageDto>(this.baseUrl + 'api/document/inviteclient', bodyData, { headers: headers });
    }

    
    acceptInvitation(invitationId: number): Observable<MessageDto> {
        let bodyData = JSON.stringify({invitationId: invitationId});
        const headers = this.jwt_auth_content_type_json().headers;

        return this.http.post<MessageDto>(this.baseUrl + 'api/document/acceptinvitation', bodyData, { headers: headers });
    }

    kickViewDocument(model: OtherClientDocumentIdsDto): Observable<MessageDto> {
        let bodyData = JSON.stringify(model);
        const headers = this.jwt_auth_content_type_json().headers;

        return this.http.post<MessageDto>(this.baseUrl + 'api/document/kickviewdocument', bodyData, { headers: headers });
    }

    kickEditDocument(model: OtherClientDocumentIdsDto): Observable<MessageDto> {
        let bodyData = JSON.stringify(model);
        const headers = this.jwt_auth_content_type_json().headers;

        return this.http.post<MessageDto>(this.baseUrl + 'api/document/kickeditdocument', bodyData, { headers: headers });
    }

    getViewersForDocument(documentId: number) : Observable<ClientDto[]> {
        const headers = this.jwt_auth_content_type_json().headers;
        const params = new HttpParams().set("documentId", documentId.toString())

        return this.http.get<ClientDto[]>(this.baseUrl + 'api/document/getviewersfordocument', { headers: headers, params: params });
    }

    getEditersForDocument(documentId: number) : Observable<ClientDto[]> {
        const headers = this.jwt_auth_content_type_json().headers;
        const params = new HttpParams().set("documentId", documentId.toString())

        return this.http.get<ClientDto[]>(this.baseUrl + 'api/document/geteditersfordocument', { headers: headers, params: params });
    }



}