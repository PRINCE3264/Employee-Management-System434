
import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { IDepartment } from '../types/department';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment.development';
import { IEmployee, ICreateEmployee } from '../types/employee';
import { HttpParams } from '@angular/common/http';
@Injectable({
  providedIn: 'root'
})
export class HttpService {
  private http = inject(HttpClient);
  //private apiUrl = 'https://localhost:7197/api/Department';
  //private apiUrls = 'https://localhost:7197/api/Employee';


  getDepartments(): Observable<IDepartment[]> {
    return this.http.get<IDepartment[]>(environment.apiUrl);
  }
  addDepartment(name: string): Observable<IDepartment> {
    return this.http.post<IDepartment>(environment.apiUrl, { name });
  }
  updateDepartment(id: number, name: string): Observable<IDepartment> {
    return this.http.put<IDepartment>(`${environment.apiUrl}/${id}`, { name });
  }
  deleteDepartment(id: number): Observable<void> {
    return this.http.delete<void>(`${environment.apiUrl}/${id}`);
  }
  getEmployeeList(filter: any): Observable<{ data: IEmployee[]; totalCount: number }> {
    const params = new HttpParams({ fromObject: filter });
    return this.http.get<{ data: IEmployee[]; totalCount: number }>(environment.apiUrls, { params });
  }


  addEmployee(employee: IEmployee) {
    return this.http.post(`${environment.apiUrls}`, employee);
  }

  updateEmployee(id: number, employee: IEmployee) {
    return this.http.put(`${environment.apiUrls}/${id}`, employee);
  }
  getEmployeeById(id: number) {
    return this.http.get<IEmployee>(`${environment.apiUrls}/${id}`);

  }
  deleteEmployee(id: number) {
    return this.http.delete(`${environment.apiUrls}/${id}`);
  }

}

