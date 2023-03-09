import Styles from "./CommentReplyForm.module.scss";
import React from "react";

export const Toolbar = ({ AddHtmlTag }:{ AddHtmlTag:(tagOpen: string, tagClose: string) => void }) => {
    return <ul className={Styles.Toolbar}>
        <li>
            <button className={Styles.ToolbarButton} onClick={() => AddHtmlTag(`<a href="">`, `</a>`)} tabIndex={-1} type="button">
                <svg className={Styles.ToolbarButtonSvg} viewBox="0 0 40 40" role="img">
                    <path d="M7.274 19.152l-2.426-2.426 4.855-4.853-2.426-2.424-7.277 7.277 7.274 7.274 7.267-7.269-2.424-2.424zM19.152 2.424l-2.426-2.424-7.267 7.269 2.424 2.424 4.843-4.845 2.426 2.426-4.855 4.853 2.426 2.424 7.277-7.277zM8.862 13.94l4.849-4.849 1.212 1.212-4.849 4.849-1.212-1.212z"/>
                </svg>
            </button>
        </li>
        <li>
            <button className={Styles.ToolbarButton} onClick={() => AddHtmlTag(`<i>`, `</i>`)} tabIndex={-1} type="button">
                <svg className={Styles.ToolbarButtonSvg} viewBox="0 0 40 40" role="img">
                    <path d="M3.996 24h-4l4.004-24h4z"/>
                </svg>
            </button>
        </li>
        <li>
            <button className={Styles.ToolbarButton} onClick={() => AddHtmlTag(`<strong>`, `</strong>`)} tabIndex={-1} type="button">
                <svg className={Styles.ToolbarButtonSvg} viewBox="0 0 40 40" role="img">
                    <path d="M15.39 11.282l-.354-.254c1.076-1.122 1.64-2.532 1.64-4.118 0-2.282-1.018-4.128-2.94-5.336-1.676-1.06-3.922-1.574-6.866-1.574h-6.87v24h7.838c2.854 0 5.202-.536 6.98-1.59 2.082-1.236 3.182-3.24 3.182-5.79 0-2.214-.902-4.06-2.61-5.338zm-11.39-7.282c1.13 0 2.26-.048 3.39-.006 1.308.05 2.714.15 3.88.812.56.318 1.1.81 1.304 1.438.196.606.046 1.302-.322 1.814-.454.634-1.192 1.016-1.952 1.192-.932.216-1.902.15-2.85.15h-3.4499999999999997v-5.4zm8.78 14.98c-.414.264-.89.44-1.364.572-1.17.328-2.366.448-3.578.448h-3.838v-6.6h3.238c1.644 0 3.392-.126 4.932.572.618.28 1.182.718 1.514 1.308.7 1.244.298 2.93-.904 3.7z"/>
                </svg>
            </button>
        </li>
        <li>
            <button className={Styles.ToolbarButton} onClick={() => AddHtmlTag(`<code>`, `</code>`)} tabIndex={-1} type="button">
                <svg className={Styles.ToolbarButtonSvg} viewBox="0 0 40 40" role="img">
                    <path d="M9.698 2.343l-9.698 9.696 9.698 9.698 2.424-2.424-7.274-7.274 7.274-7.272zM13.716 24h3.429l3.429-24h-3.429zM31.913 9.615l-7.274-7.272-2.424 2.424 7.274 7.272-7.274 7.274 2.424 2.424 9.698-9.698z"/>
                </svg>
            </button>
        </li>
    </ul>
}
