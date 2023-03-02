export interface IComment {
    Id: number,
    UserName: string,
    Email: string,
    Text: string,
    ParentId: number | null,
    Parent: Comment | null,
    Files: File[],
    DateAdded: string
    Comments: IComment[]
}