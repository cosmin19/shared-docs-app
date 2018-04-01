import { BrowserModule } from '@angular/platform-browser';
import { NgModule, CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';

import { AppRoutingModule } from './app-routing.module';

import { AppComponent } from './app.component';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { LoginComponent } from './login/login.component';
import { RegisterComponent } from './register/register.component';
import { AppContentComponent } from './app-content.component';
import { AuthenticationService } from './_services/authentication.service';
import { AlertService } from './_services/alert.service';
import { GreaterThanZeroValidator, EqualValidator, EmailValidator } from './_directives';
import { AppConfig } from './app.config';
import { HeaderComponent } from './user/header/header.component';
import { UserContentComponent } from './user/user-content';
import { UserComponent } from './user/user.component';
import { HttpClient, HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { HttpModule } from '@angular/http';
import { CommonModule } from '@angular/common';
import { ChipsModule } from 'primeng/components/chips/chips';
import { DataListModule } from 'primeng/components/datalist/datalist';
import { GrowlModule } from 'primeng/components/growl/growl';
import { RouterModule } from '@angular/router';
import { MessageService } from 'primeng/components/common/messageservice';
import { AuthGuard } from './_guards';
import { DocumentComponent } from './document/document.component';
import { DocumentInfoComponent } from './document/info/info.component';
import { AddDocumentComponent } from './document/modal/add-document/add-document.component';
import { DocumentService } from './_services/document.service';
import { EditorModule } from 'primeng/components/editor/editor';
import { MiniInfoComponent } from './document/mini-info/mini-info.component';
import { NgProgressModule, NgProgressInterceptor } from 'ngx-progressbar';
import { ConfirmationService } from 'primeng/components/common/confirmationservice';
import { ConfirmDialogModule } from 'primeng/confirmdialog';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { EditDocumentComponent } from './document/modal/edit-document/edit-document.component';

@NgModule({
    declarations: [
        AppComponent,
        LoginComponent,
        RegisterComponent,
        AppContentComponent,
        GreaterThanZeroValidator,
        EmailValidator,
        EqualValidator,
        HeaderComponent,
        UserContentComponent,
        UserComponent,
        DocumentComponent,
        DocumentInfoComponent,
        AddDocumentComponent,
        MiniInfoComponent,
        EditDocumentComponent,
    ],
    imports: [
        BrowserModule,
        FormsModule,
        BrowserAnimationsModule,
        ConfirmDialogModule,
        ReactiveFormsModule,
        NgProgressModule,

        HttpModule,
        HttpClientModule,

        CommonModule,

        /* PrimeNG */
        ChipsModule,
        DataListModule,
        GrowlModule,
        EditorModule,
        AppRoutingModule,
    ],
    providers: [
        AuthenticationService,
        AlertService,
        AppConfig,
        MessageService,
        DocumentService,
        ConfirmationService,
        AuthGuard,
        { provide: HTTP_INTERCEPTORS, useClass: NgProgressInterceptor, multi: true },

    ],
    bootstrap: [AppComponent],
    schemas: [CUSTOM_ELEMENTS_SCHEMA]
})
export class AppModule { }
