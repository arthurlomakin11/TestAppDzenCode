import React, {useEffect, useState} from "react";
import Styles from "./Comment.module.scss"
import {IComment} from "../../interfaces/IComment";
import HomeStyles from "../Home/Home.module.scss";
import {Link} from "react-router-dom";
import {CommentReplyForm} from "../CommentReplyForm/CommentReplyForm";

export let Comment = ({comment, homePageView = false, onReplyOpenButtonClick}:{comment:IComment, homePageView?: boolean, onReplyOpenButtonClick?: (c:number) => void}) => {
    let addReplyEvent = (c: IComment) => {
        setComments([...comment.Comments, c]);
    }
    const [comments, setComments] = useState<IComment[]>([]);
    useEffect(() => {
        setComments(comment?.Comments);
    }, [])
    
    return <div className={Styles.Comment}>
        <div className={Styles.CommentHead}>
            {
                !homePageView ? <>
                    <span className={Styles.CommentUser}>{comment.UserName}</span>

                    <time className={Styles.CommentDateTime}>{new Date(comment.DateAdded).toLocaleString()}</time>

                    <div className={Styles.CommentMessage}>{comment.Text}</div>

                    <div className={Styles.CommentFooter}>
                        <button onClick={onReplyOpenButtonClick ? () => onReplyOpenButtonClick(comment.Id) : () => {}} className={Styles.CommentFooterLink}>Ответить</button>
                    </div>
                    <CommentReplyForm id={comment.Id} addReplyEvent={addReplyEvent} onReplyOpenButtonClick={onReplyOpenButtonClick ? onReplyOpenButtonClick : () => {}}/>
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