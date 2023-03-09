export enum FileType {
    Image,
    TextFile
}

export interface IFile {
    Id: number,
    Src: string,
    FileType: FileType
}