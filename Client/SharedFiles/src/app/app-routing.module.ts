import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { LoginComponent } from './login/login.component';
import { RegisterComponent } from './register/register.component';
import { AppContentComponent } from './app-content.component';
import { DashboardComponent } from './user/dashboard';
import { ClientsComponent } from './user/clients';
import { UserComponent } from './user/user.component';
import { AuthGuard } from './_guards';

const routes: Routes =
    [
        { path: 'login', component: LoginComponent },
        { path: 'register', component: RegisterComponent },
        {
            path: '', component: UserComponent,
            children: [
                { path: 'dashboard', component: DashboardComponent },
                { path: 'clients', component: ClientsComponent },
                { path: '', component: DashboardComponent },
            ],
            canActivate:[AuthGuard]
        }
    ];

@NgModule({
    imports: [RouterModule.forRoot(routes)],
    exports: [RouterModule]
})
export class AppRoutingModule { }
