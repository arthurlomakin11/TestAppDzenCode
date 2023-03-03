import React from "react";
import Styles from "./Comment.module.scss"
import {IComment} from "../../interfaces/IComment";
import HomeStyles from "../Home/Home.module.scss";

export let Comment = ({comment, showOnlyText = false}:{comment:IComment, showOnlyText?: boolean}) => {
    return <div className={Styles.Comment}>
        <div className={Styles.CommentHead}>
            {
                !showOnlyText ? <>
                    <span className={Styles.CommentUser}>{comment.UserName}</span>

                    <time className={Styles.CommentDateTime}>{comment.DateAdded?.toLocaleString()}</time>
                </> : <></>
            }

            <div className={Styles.CommentMessage}>{comment.Text}</div>

            {
                !showOnlyText ? <>
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