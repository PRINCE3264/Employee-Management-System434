export interface AuthTokenDto {
  id: number;
  email: string;
  token: string;
  //role: string;
  role: 'Admin' | 'Employee';
}
