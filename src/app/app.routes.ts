import { Routes } from '@angular/router';
import { HomeComponent } from './pages/home/home.component';
import { DepartmentsComponent } from './pages/departments/departments.component';
import { EmployeeComponent } from './pages/employee/employee.component';
import { LoginComponent } from './pages/login/login.component';
import { EmployeedashboardComponent } from './pages/employeedashboard/employeedashboard.component';
import { AboutComponent } from './pages/about/about.component';
import { ContactComponent } from './pages/contact/contact.component';
import { ProfileComponent } from './pages/profile/profile.component';
import { LeaveComponent } from './pages/leave/leave.component';
import { AttendanceComponent } from './pages/attendance/attendance.component';

export const routes: Routes = [
  {
    path: "",
    component: HomeComponent
  },
  {
    path: "employee-dashboard",
    component: EmployeedashboardComponent
  },
  {
    path: "departments",
    component: DepartmentsComponent
  },
  {
    path: "employee",
    component: EmployeeComponent
  },
  {
    path: "login",
    component: LoginComponent
  },
  {
    path: "about",
    component: AboutComponent
  },
  {
    path: "contact",
    component: ContactComponent,
  },
  {
    path: "profile",
    component: ProfileComponent,
  },
  {
    path: "leaves",
    component: LeaveComponent,
  },
  {
    path: "attendance",
    component: AttendanceComponent,
  },
  {
    path: "attendance/:id",
    component: AttendanceComponent,
  }
];
