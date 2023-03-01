export interface IComment {
    id: number,
    userName: string,
    email: string,
    text: string,
    parentId: number | null,
    parent: Comment | null,
    files: File[],
    dateAdded: Date
}