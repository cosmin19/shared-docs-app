import { Component, OnInit, EventEmitter, Output } from '@angular/core';
import { FormGroup, FormBuilder, FormControl } from '@angular/forms';
import { AlertService } from '../../../_services';
import { Router } from '@angular/router';
import { DocumentService } from '../../../_services/document.service';
import { DocumentNewDto } from '../../../_models/document/document-new';

@Component({
  selector: 'add-document',
  templateUrl: './add-document.component.html',
  styleUrls: ['./add-document.component.css']
})
export class AddDocumentComponent implements OnInit {

    addDocumentForm: FormGroup;
    documentNew: DocumentNewDto = new DocumentNewDto();
    @Output() documentEmitter: EventEmitter<boolean> = new EventEmitter<boolean>();
    
    constructor(fb: FormBuilder,
        private router: Router,
        private _alertService: AlertService,
        private _documentService: DocumentService
    ) {
        this.addDocumentForm = fb.group({
            title: [null],
            subject: [null]
        });
    }

    ngOnInit() {
    
    }

    submit() {
        this.documentNew.title = this.title.value;
        this.documentNew.subject = this.subject.value;
        this._documentService.addNewDocument(this.documentNew)
        .subscribe(
            data => {
                if(data.success){
                    this._alertService.success("Document", data.message);
                    this.documentEmitter.emit(true);
                    this.clearForm();
                    this.router.navigate(['']);
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
        this.addDocumentForm.reset();
    }

    /* GETTERS */
    get title() {
        return this.addDocumentForm.get('title') as FormControl;
    }
    get subject() {
        return this.addDocumentForm.get('subject') as FormControl;
    }
}
