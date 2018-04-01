import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { LoginComponent } from './login/login.component';
import { RegisterComponent } from './register/register.component';
import { AppContentComponent } from './app-content.component';
import { UserComponent } from './user/user.component';
import { AuthGuard } from './_guards';
import { DocumentComponent } from './document/document.component';
import { DocumentInfoComponent } from './document/info/info.component';

const routes: Routes =
    [
        { path: 'login', component: LoginComponent },
        { path: 'register', component: RegisterComponent },
        {
            path: '', component: UserComponent,
            children: [
                { path: '', component: DocumentComponent },
                { path: 'document/:id', component: DocumentInfoComponent },
            ],
            canActivate:[AuthGuard]
        }
    ];

@NgModule({
    imports: [RouterModule.forRoot(routes)],
    exports: [RouterModule]
})
export class AppRoutingModule { }
