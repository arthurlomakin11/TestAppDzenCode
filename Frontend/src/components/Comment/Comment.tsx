import React, {useEffect, useState} from "react";
import Styles from "./Comment.module.scss"
import {IComment} from "../../interfaces/IComment";
import HomeStyles from "../Home/Home.module.scss";
import {Link} from "react-router-dom";
import {CommentReplyForm} from "../CommentReplyForm/CommentReplyForm";
import {FileType, IFile} from "../../interfaces/IFile";
import FsLightbox from "fslightbox-react";

export let Comment = ({comment, homePageView = false, onReplyOpenButtonClick, preview = false}:{
    comment:IComment, 
    homePageView?: boolean, 
    onReplyOpenButtonClick?: (c:number) => void,
    preview?: boolean
}) => {
    let addReplyEvent = (c: IComment) => {
        setComments([...comments, c]);
    }
    const [comments, setComments] = useState<IComment[]>([]);
    
    useEffect(() => {
        if(comment?.Comments) {
            setComments(comment.Comments)
        }
    }, [])
    
    return <div className={`${Styles.Comment} ${preview ? Styles.CommentPreview : ""}`}>
        <div className={Styles.CommentHead}>
            {
                !homePageView ? <>
                    <span className={Styles.CommentUser}>{comment.UserName}</span>

                    <time className={Styles.CommentDateTime}>{new Date(comment.DateAdded).toLocaleString()}</time>

                    <div className={Styles.CommentMessage} dangerouslySetInnerHTML={{__html: comment.Text}}/>

                    {
                        comment.Files ? <Files files={comment.Files}/> : <></>
                    }
                    
                    {
                        !preview ? <>
                            <div className={Styles.CommentFooter}>
                                <button onClick={onReplyOpenButtonClick ? () => onReplyOpenButtonClick(comment.Id) : () => {}} className={Styles.CommentFooterLink}>Ответить</button>
                            </div>
                            <CommentReplyForm id={comment.Id} addReplyEvent={addReplyEvent} onReplyOpenButtonClick={onReplyOpenButtonClick ? onReplyOpenButtonClick : () => {}}/>
                        </> : <></>
                    }
                </> : <>
                    <div className={Styles.CommentMessage}>
                        <Link to={`./comment/${comment.Id}`}>
                            {comment.Text}
                        </Link>
                    </div>
                </>
            }

            {
                comments ? <ul className={HomeStyles.ContentListNestedComments}>
                    <li className={HomeStyles.ContentListItem}>
                        {
                            comments.map(children => {
                                return <Comment key={children.Id} comment={children} onReplyOpenButtonClick={onReplyOpenButtonClick}/>
                            })
                        }
                    </li>
                </ul> : <></>
            }
        </div>
    </div>
}

const Files = ({files}:{files:IFile[]}) => {
    let imageCounter = 1;
    function getImageCounter() {
        return imageCounter++;
    }
    
    const imagesSrcList = files.filter(f => f.FileType == FileType.Image).map(f => f.Src);
    
    return <>
        <ul className={Styles.FileList}>
            {
                files.map(file => {
                    return <li className={Styles.FileListItem} key={file.Id}>
                        {
                            file.FileType == FileType.Image ? 
                                <CommentsImage imageSrc={file.Src} imagesSrcList={imagesSrcList} imageCountNumber={getImageCounter()}/> 
                                : <a href={file.Src}>
                                    <img className={Styles.FileListSvg} src={"./File.svg"} alt="file icon svg"/>
                                </a>
                        }
                    </li>
                })
            }
        </ul>
    </>
}

const CommentsImage = ({imageSrc, imagesSrcList, imageCountNumber}:{imageSrc:string, imagesSrcList:string[], imageCountNumber:number}) => {
    const [toggler, setToggler] = useState(false);
    
    return <>
        <img className={Styles.FileListImg} 
             src={imageSrc}
             onClick={() => setToggler(!toggler)}/>
        <FsLightbox toggler={toggler}
                    slide={imageCountNumber} 
                    sources={imagesSrcList}/>
    </>
}