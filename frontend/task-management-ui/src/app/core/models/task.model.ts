export interface TaskItem {
  id: number;
  title: string;
  description?: string;
  status: string;
  userId: number;
  extraData?: string;
  createDate: string;
  priority:string;
}
