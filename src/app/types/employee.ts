

/*export interface IEmployee {
  id: number;
  name: string;
  email: string;
  phone: string;
  jobTitle: string;
  gender: string;
  departmentId: number;
  joiningDate: string;        // Format: YYYY-MM-DDTHH:mm:ss (ISO 8601)
  lastWorkingDate: string;
  dateOfBirth: string;
}

*/

// ✅ src/app/types/employee.ts

/*export interface IEmployee {
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

*/
export interface ICreateEmployee {
  name: string;
  email: string;
  phone?: string;
  jobTitle: string;
  gender: string;
  departmentId: number;
  joiningDate: string;      // Use string to match ISO format from date picker
  lastWorkingDate: string;
  dateOfBirth: string;
}

export interface IEmployee extends ICreateEmployee {
  id: number;
}

