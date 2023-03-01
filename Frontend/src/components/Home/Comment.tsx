import React, {ReactNode} from "react";
import Styles from "./Comment.module.scss"

interface Props {
    children: ReactNode
}

export let Comment : React.FC<Props> = props => {
    return <div className={Styles.Comment}>
        <div className={Styles.CommentHead}>
            <span className={Styles.CommentUser}>JordanCpp</span>

            <time className={Styles.CommentDateTime}>February 17, 2023 at 04:10 PM</time>

            <div className={Styles.CommentMessage}>
                Но потом я подумал о том, что с таким названием статью даже не откроют, поэтому решился
                    пойти на эту маленькую хитрость с переименованием.&nbsp;
                Главная проблема статьи, её сложно осилить:)
            </div>

            <div className={Styles.CommentFooter}>
                <a href="" className={Styles.CommentFooterLink}>Ответить</a>
            </div>

            <div className={Styles.CommentReplyForm}/>

            {props.children}
        </div>
    </div>
}