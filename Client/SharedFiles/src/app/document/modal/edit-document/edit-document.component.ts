import { Component, OnInit, Input, Output, SimpleChanges, OnChanges, EventEmitter } from '@angular/core';
import { FormGroup, FormBuilder, FormControl } from '@angular/forms';
import { DocumentNewDto } from '../../../_models/document/document-new';
import { Router } from '@angular/router';
import { AlertService } from '../../../_services';
import { DocumentService } from '../../../_services/document.service';
import { DocumentEditDto } from '../../../_models/document/document-edit';
import { DocumentMiniInfo } from '../../../_models/document/document-mini-info';

@Component({
    selector: 'edit-document',
    templateUrl: './edit-document.component.html',
    styleUrls: ['./edit-document.component.css']
})
export class EditDocumentComponent implements OnInit, OnChanges {
    @Input() document: DocumentMiniInfo;
    editDocumentForm: FormGroup;
    documentEdit: DocumentEditDto = new DocumentEditDto();
    @Output() documentEmitter: EventEmitter<boolean> = new EventEmitter<boolean>();
    


    constructor(fb: FormBuilder,
        private router: Router,
        private _alertService: AlertService,
        private _documentService: DocumentService
    ) {
        this.editDocumentForm = fb.group({
            title: [null],
            subject: [null]
        });
    }

    ngOnInit() {
        this.editDocumentForm.get('title').setValue(this.document.title);
        this.editDocumentForm.get('subject').setValue(this.document.subject);

        this.documentEdit.id = this.document.id;
        this.documentEdit.title = this.document.title;
        this.documentEdit.subject = this.document.subject;
    }

    ngOnChanges(changes: SimpleChanges) {
        this.editDocumentForm.get('title').setValue(this.document.title);
        this.editDocumentForm.get('subject').setValue(this.document.subject);

        this.documentEdit.id = this.document.id;
        this.documentEdit.title = this.document.title;
        this.documentEdit.subject = this.document.subject;
    
    }

    submit() {
        this.documentEdit.title = this.title.value;
        this.documentEdit.subject = this.subject.value;
        this._documentService.editDocument(this.documentEdit)
            .subscribe(
                data => {
                    if (data.success) {
                        this._alertService.success("Document", data.message);
                        this.documentEmitter.emit(true);
                    }
                    else {
                        this._alertService.error("Document", data.message);
                    }
                },
                error => {
                    var message = error.message;
                    this._alertService.error("Document", message);
                }
            );
    }

    clearForm(): void {
        this.editDocumentForm.reset();
    }

    /* GETTERS */
    get title() {
        return this.editDocumentForm.get('title') as FormControl;
    }
    get subject() {
        return this.editDocumentForm.get('subject') as FormControl;
    }
}
