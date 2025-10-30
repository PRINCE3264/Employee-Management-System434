
export interface IAttendance {
  type: AttendanceType;
  date: string
}


export enum AttendanceType {
  Present = 1,
  Leave = 2

}
