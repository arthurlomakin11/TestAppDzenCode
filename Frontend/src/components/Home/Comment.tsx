import React, {ReactNode} from "react";
import Styles from "./Comment.module.scss"
import {IComment} from "../../interfaces/IComment";


interface Props {
    children: ReactNode,
    comment: IComment
}

export let Comment : React.FC<Props> = props => {
    return <div className={Styles.Comment}>
        <div className={Styles.CommentHead}>
            <span className={Styles.CommentUser}>{props.comment.userName}</span>

            <time className={Styles.CommentDateTime}>{props.comment.dateAdded?.toLocaleString()}</time>

            <div className={Styles.CommentMessage}>{props.comment.text}</div>

            <div className={Styles.CommentFooter}>
                <a href="" className={Styles.CommentFooterLink}>Ответить</a>
            </div>

            <div className={Styles.CommentReplyForm}/>

            {props.children}
        </div>
    </div>
}