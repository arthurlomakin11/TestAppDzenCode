import {IFile} from "./IFile";

export interface IComment {
    Id: number,
    UserName: string,
    Email: string,
    Text: string,
    ParentId: number | null,
    Parent: Comment | null,
    Files: IFile[],
    DateAdded: string
    Comments: IComment[]
}