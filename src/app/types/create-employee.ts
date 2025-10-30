// employee.ts
/*export interface ICreateEmployee {
  name: string;
  email: string;
  phone?: string;
  jobTitle: string;
  gender: string;
  departmentId: number;
  joiningDate: string;
  lastWorkingDate: string;
  dateOfBirth: string;
}

export interface IEmployee extends ICreateEmployee {
  id: number;
}
*/

export interface IEmployee {
  id: number,
  name: string;
  email: string;
  phone: string;
  jobTitle: string;
  gender: string;
  departmentId: number;
  joiningDate: string;
  lastWorkingDate: string;
  dateOfBirth: string;
}
