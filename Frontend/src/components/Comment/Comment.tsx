import React from "react";
import Styles from "./Comment.module.scss"
import {IComment} from "../../interfaces/IComment";
import HomeStyles from "../Home/Home.module.scss";
import {Link} from "react-router-dom";
import {CommentReplyForm} from "../CommentReplyForm/CommentReplyForm";

export let Comment = ({comment, homePageView = false, onReplyButtonClick}:{comment:IComment, homePageView?: boolean, onReplyButtonClick?: (c:IComment) => void}) => {
    return <div className={Styles.Comment}>
        <div className={Styles.CommentHead}>
            {
                !homePageView ? <>
                    <span className={Styles.CommentUser}>{comment.UserName}</span>

                    <time className={Styles.CommentDateTime}>{comment.DateAdded?.toLocaleString()}</time>

                    <div className={Styles.CommentMessage}>{comment.Text}</div>

                    <div className={Styles.CommentFooter}>
                        <a onClick={() => onReplyButtonClick ? onReplyButtonClick(comment) : {}} className={Styles.CommentFooterLink}>Ответить</a>
                    </div>
                    <CommentReplyForm id={comment.Id}/>
                </> : <>
                    <div className={Styles.CommentMessage}>
                        <Link to={`./comment/${comment.Id}`}>
                            {comment.Text}
                        </Link>
                    </div>
                </>
            }

            {
                comment.Comments ? <ul className={HomeStyles.ContentListNestedComments}>
                    <li className={HomeStyles.ContentListItem}>
                        {
                            comment.Comments.map(children => {
                                return <Comment key={children.Id} comment={children} onReplyButtonClick={onReplyButtonClick}/>
                            })
                        }
                    </li>
                </ul> : <></>
            }
        </div>
    </div>
}