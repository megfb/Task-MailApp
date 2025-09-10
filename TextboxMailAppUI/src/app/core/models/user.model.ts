export interface UserModel {
  userName: string | null;
  passwordHash: string | null;
  emailAddress: string | null;
  emailPassword?: string | null;
  serverName?: string | null;
  port?: number | null;
}
