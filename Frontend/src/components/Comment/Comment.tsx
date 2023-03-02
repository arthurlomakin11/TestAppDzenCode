import React from "react";
import Styles from "./Comment.module.scss"
import {IComment} from "../../interfaces/IComment";
import HomeStyles from "../Home/Home.module.scss";

export let Comment = ({comment, showReplyButton = true}:{comment:IComment, showReplyButton?: boolean}) => {
    return <div className={Styles.Comment}>
        <div className={Styles.CommentHead}>
            <span className={Styles.CommentUser}>{comment.UserName}</span>

            <time className={Styles.CommentDateTime}>{comment.DateAdded?.toLocaleString()}</time>

            <div className={Styles.CommentMessage}>{comment.Text}</div>

            {
                showReplyButton ? <>
                    <div className={Styles.CommentFooter}>
                        <a href="" className={Styles.CommentFooterLink}>Ответить</a>
                    </div>
                    <div className={Styles.CommentReplyForm}/>
                </> : <></>
            }

            {
                comment.Comments ? <ul className={HomeStyles.ContentListNestedComments}>
                    <li className={HomeStyles.ContentListItem}>
                        {
                            comment.Comments.map(children => {
                                return <Comment key={children.Id} comment={children}/>
                            })
                        }
                    </li>
                </ul> : <></>
            }
        </div>
    </div>
}