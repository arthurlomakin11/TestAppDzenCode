import Styles from "./CommentReplyForm.module.scss";
import React, {ChangeEvent, FormEvent, MouseEvent, useContext, useRef, useState} from "react";
import {ReplyFormCommentIdContext} from "./ReplyFormContext";
import axios, {AxiosHeaders} from "axios";
import {IComment} from "../../interfaces/IComment";
import ReCAPTCHA from "react-google-recaptcha";
import {PUBLIC_RECAPTCHA_SITE_KEY} from "../../globalVariables";
import sanitizeHtml from "sanitize-html";
import {Comment} from "../Comment/Comment";
import {Toolbar} from "./Toolbar";

export let CommentReplyForm = ({id, addReplyEvent, onReplyOpenButtonClick}:{id:number, addReplyEvent?: (c: IComment) => void, onReplyOpenButtonClick: (c:number) => void}) => {
    const [message, setMessage] = useState('');
    const ReplyFormCommentId:number = useContext(ReplyFormCommentIdContext);
    const reRef = useRef<ReCAPTCHA>(null);
    const [selectionStart, setSelectionStart] = useState<number>(0);
    const [selectionEnd, setSelectionEnd] = useState<number>(0);
    const [email, setEmail] = useState<string>("");
    const [userName, setUserName] = useState<string>("");
    const [previewComment, setPreviewComment] = useState<IComment | null>(null);
    const [fileList, setFileList] = useState<FileList | null>(null);

    const handleFileChange = (e: ChangeEvent<HTMLInputElement>) => {
        setFileList(e.target.files);
    };
        
    const handleMessageChange = (event: ChangeEvent<HTMLTextAreaElement>) => {
        setMessage(event.target.value);
    };

    const handleTextAreaClick = (event: MouseEvent<HTMLTextAreaElement>) => {
        setSelectionStart(event.currentTarget.selectionStart);
        setSelectionEnd(event.currentTarget.selectionEnd);
    };
    
    const AddHtmlTag = (tagOpen:string, tagClose:string) => {
        let newMessage = message.slice(0, selectionStart);
        newMessage += tagOpen + message.slice(selectionStart, selectionEnd) + tagClose;
        newMessage += message.slice(selectionEnd, message.length);
        setMessage(newMessage);
    }

    const handleSubmit = async (event: FormEvent) => {
        event.preventDefault();
        
        const token = reRef?.current?.getValue();
        
        const response = await axios.postForm<IComment>("api/comment/reply",{
            RootCommentId: id,
            UserName: userName,
            Email: email,
            Text: message,
            Token: token,
            Files: fileList
        });
        reRef?.current?.reset();
        if(response.status == 200) {
            addReplyEvent ? addReplyEvent(response.data) : (() => {})();
            onReplyOpenButtonClick(0);
            setMessage("");
        }
    }

    const PreviewButtonOnClick = () =>  {
        setPreviewComment({
            Text: sanitizeHtml(message, {
                allowedTags: [ "i", "strong", "a", "code" ],
                allowedAttributes: {
                    "a": [ "href" ]
                }
            }),
            UserName: userName,
            DateAdded: new Date(Date.now()).toISOString(),
            Email: email,
            Id: 0,
            Comments: [], 
            Files: [], 
            Parent: null, 
            ParentId: null
        })
    }
    
    return ReplyFormCommentId == id ? <>
        <div>
            {previewComment ? <Comment comment={previewComment} preview={true}/> : <></>}
        </div>
        <form className={Styles.CommentReplyForm} onSubmit={handleSubmit}>
            <div className={Styles.InputBlock}>
                <label className={Styles.Label} htmlFor="email">Email:</label>
                <input className={Styles.Input} 
                       value={email} 
                       onChange={i => setEmail(i.currentTarget.value)} 
                       id="email" 
                       placeholder="email@gmail.com"
                       type="email"
                       required={true}
                />
            </div>
            
            <div className={Styles.InputBlock}>
                <label className={Styles.Label} htmlFor="username">Имя:</label>
                <input className={Styles.Input}
                       value={userName}
                       onChange={i => setUserName(i.currentTarget.value)}
                       id="username"
                       placeholder="Vasya"
                       pattern="^[a-zA-Z0-9]*$"
                />
            </div>
            
            <Toolbar AddHtmlTag={AddHtmlTag}/>
            
            <textarea className={Styles.TextArea} value={message} onChange={handleMessageChange} onSelect={handleTextAreaClick} required={true}/>
            
            <input type="file" onChange={handleFileChange} multiple />
            
            <ReCAPTCHA className={Styles.Captcha} sitekey={PUBLIC_RECAPTCHA_SITE_KEY} ref={reRef}/>
            
            <button className={`${Styles.Button} ${Styles.ButtonNoFill}`} onClick={PreviewButtonOnClick} type="button">Предпросмотр</button>
            <button className={`${Styles.Button} ${Styles.ButtonFill}`} type="submit">Отправить</button>
        </form>
    </> : <></>
}