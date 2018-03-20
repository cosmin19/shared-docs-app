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
import { GreaterThanZeroValidator, EqualValidator } from './_directives';
import { AppConfig } from './app.config';
import { SidebarComponent } from './user/sidebar';
import { HeaderComponent } from './user/header/header.component';
import { DashboardComponent } from './user/dashboard';
import { ClientsComponent } from './user/clients';
import { UserContentComponent } from './user/user-content';
import { UserComponent } from './user/user.component';
import { HttpClient, HttpClientModule } from '@angular/common/http';
import { HttpModule } from '@angular/http';
import { CommonModule } from '@angular/common';
import { ChipsModule } from 'primeng/components/chips/chips';
import { DataListModule } from 'primeng/components/datalist/datalist';
import { GrowlModule } from 'primeng/components/growl/growl';
import { RouterModule } from '@angular/router';
import { MessageService } from 'primeng/components/common/messageservice';
import { AuthGuard } from './_guards';

@NgModule({
    declarations: [
        AppComponent,
        LoginComponent,
        RegisterComponent,
        AppContentComponent,
        GreaterThanZeroValidator,
        EqualValidator,
        SidebarComponent,
        HeaderComponent,
        DashboardComponent,
        ClientsComponent,
        UserContentComponent,
        UserComponent,
    ],
    imports: [
        BrowserModule,
        FormsModule,
        ReactiveFormsModule,

        HttpModule,
        HttpClientModule,

        CommonModule,

        /* PrimeNG */
        ChipsModule,
        DataListModule,
        GrowlModule,
        AppRoutingModule,
    ],
    providers: [
        AuthenticationService,
        AlertService,
        AppConfig,
        MessageService,
        AuthGuard,
    ],
    bootstrap: [AppComponent],
    schemas: [CUSTOM_ELEMENTS_SCHEMA]
})
export class AppModule { }
