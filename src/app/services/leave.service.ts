

// import { HttpClient, HttpParams } from '@angular/common/http';
// import { Injectable, inject } from '@angular/core';
// import { environment } from '../../environments/environment.development';
// import { ILeave } from '../types/leave';
// import { PagedData } from '../types/paged-data';
// import { Observable } from 'rxjs';
// import { IAttendance } from '../types/attendance'; // ✅ Ensure this is imported

// @Injectable({
//   providedIn: 'root'
// })
// export class LeaveService {
//   private http = inject(HttpClient);

//   // ✅ Apply for a new leave
//   applyLeave(type: number, reason: string, leaveDate: string): Observable<ILeave> {
//     return this.http.post<ILeave>(`${environment.apiUrlLeaves}/apply`, {
//       type,
//       reason,
//       leaveDate
//     });
//   }

//   // ✅ Get paginated list of leaves
//   getLeaves(filter: any): Observable<PagedData<ILeave>> {
//     const params = new HttpParams({ fromObject: filter });
//     return this.http.get<PagedData<ILeave>>(`${environment.apiUrlLeaves}?${params.toString()}`);
//   }

//   // ✅ Cancel a leave (for user)
//   cancelLeave(id: number): Observable<void> {
//     return this.http.put<void>(`${environment.apiUrlLeaves}/cancel/${id}`, {});
//   }

//   // ✅ Accept a leave (Admin)
//   acceptLeave(id: number): Observable<void> {
//     return this.http.put<void>(`${environment.apiUrlLeaves}/accept/${id}`, {});
//   }

//   // ✅ Reject a leave (Admin)
//   rejectLeave(id: number): Observable<void> {
//     return this.http.put<void>(`${environment.apiUrlLeaves}/reject/${id}`, {});
//   }


//   getMyAttendanceHistory(filter: any) {
//     const params = new HttpParams({ fromObject: filter });
//     return this.http.get<PagedData<IAttendance>>(`${environment.apiBaseUrl}/Attendance/me`, { params });
//   }

//   getAttendanceHistoryByEmployee(employeeId: number, filter: any) {
//     const params = new HttpParams({ fromObject: filter });
//     return this.http.get<PagedData<IAttendance>>(`${environment.apiBaseUrl}/Attendance/Employee/${employeeId}`, { params });
//   }

// }


import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment.development';
import { ILeave } from '../types/leave';
import { IAttendance } from '../types/attendance';
import { PagedData } from '../types/paged-data';

@Injectable({
  providedIn: 'root'
})
export class LeaveService {
  private http = inject(HttpClient);

  // ✅ Apply for a new leave
  applyLeave(type: number, reason: string, leaveDate: string): Observable<ILeave> {
    return this.http.post<ILeave>(`${environment.apiUrlLeaves}/apply`, {
      type,
      reason,
      leaveDate
    });
  }

  // ✅ Get paginated list of leaves (Employee/Admin)
  getLeaves(filter: any): Observable<PagedData<ILeave>> {
    const params = new HttpParams({ fromObject: filter });
    return this.http.get<PagedData<ILeave>>(`${environment.apiUrlLeaves}`, { params });
  }

  // ✅ Cancel a leave (Employee)
  cancelLeave(id: number): Observable<void> {
    return this.http.put<void>(`${environment.apiUrlLeaves}/cancel/${id}`, {});
  }

  // ✅ Accept a leave (Admin)
  acceptLeave(id: number): Observable<void> {
    return this.http.put<void>(`${environment.apiUrlLeaves}/accept/${id}`, {});
  }

  // ✅ Reject a leave (Admin)
  rejectLeave(id: number): Observable<void> {
    return this.http.put<void>(`${environment.apiUrlLeaves}/reject/${id}`, {});
  }

  // ✅ Get current logged-in employee's attendance
  getMyAttendanceHistory(filter: any): Observable<PagedData<IAttendance>> {
    const params = new HttpParams({ fromObject: filter });
    return this.http.get<PagedData<IAttendance>>(`${environment.apiBaseUrl}/Attendance/me`, { params });
  }

  // ✅ Get attendance for specific employee (Admin or self if allowed)
  getAttendanceHistoryByEmployee(employeeId: number, filter: any): Observable<PagedData<IAttendance>> {
    const params = new HttpParams({ fromObject: filter });
    return this.http.get<PagedData<IAttendance>>(`${environment.apiBaseUrl}/Attendance/Employee/${employeeId}`, { params });
  }
}
