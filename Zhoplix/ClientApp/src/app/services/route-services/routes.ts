import { Routes } from "@angular/router";
import { HomeComponent } from "src/app/components/home/home.component";
import { AdminComponent } from "src/app/components/admin/admin.component";
import { LoginComponent } from "src/app/components/authentication/login/login.component";
import { AuthGuardService } from "./auth-guard/auth-guard.service";
import { RoleGuardService } from "./role-guard/role-guard.service";
import { Roles } from "src/app/models/roles";
import { RegistrationComponent } from "src/app/components/authentication/registration/registration.component";

export const ROUTES: Routes = [
  { path: '', component: HomeComponent, pathMatch: 'full'},
  { path: 'login', component: LoginComponent, pathMatch: 'full'},
  { path: 'signup', component: RegistrationComponent, pathMatch: 'full'},
  {
    path: 'admin',
    component: AdminComponent,
    canActivate: [RoleGuardService],
    data: { 
      expectedRole: Roles.Admin
    }
  },
  { path: '**', redirectTo: ''}
]
