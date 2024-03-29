import React, {useEffect, useState} from 'react';
import {Comment} from "../Comment/Comment";
import {IComment} from "../../interfaces/IComment";
import {Pagination, PaginationItem, PaginationLink} from "reactstrap";
import {useParams} from "react-router-dom";
import axios from "axios";
import HomeStyles from "../Home/Home.module.scss";

export let Home = () =>  {
    const [state, setState] = useState({
        comments: [] as IComment[],
        loading: true
    })
    
    const [pagesNumber, setPagesNumber] = useState(1);
    const [orderBySelected, setOrderBySelected] = useState({
        UserName: -1, // means no order
        Email: -1,
        DateAdded: 1 // means descending order
    })

    const [currentPage, setCurrentPage] = useState(1);
    
    useEffect(() => {
        (async () => {
            const pagesNumberResponse = await axios.get<number>("api/comments/getCommentsPagesNumber");
            const pagesNumberData = pagesNumberResponse.data;
            
            setPagesNumber(pagesNumberData);
        })()
    }, [])
    
    useEffect(() => {
        (async () => {
            const data = await axios.get<IComment[]>("api/comments", {
                params: {
                    skipPage: (currentPage - 1),
                    ...orderBySelected
                }
            });

            setState({
                comments: data.data,
                loading: false
            });
        })()
    }, [orderBySelected, currentPage])
    
    
    const previousPage = currentPage > 1 ? currentPage - 1 : currentPage;
    const nextPage = currentPage < pagesNumber ? currentPage + 1 : currentPage;
    const allPages = [...Array(pagesNumber).keys()].map(i => i + 1);
    const current5PageBlock = Math.ceil(currentPage / 5);
    const PageChangerArray = allPages.slice((current5PageBlock - 1) * 5, current5PageBlock * 5);
    
    function changeOrderNumber(orderNum:number) {
        if((orderNum === -1) || (orderNum === 1)) {
            return 0;
        }
        else {
            return 1;
        }
        // If there is no order or desc, set asc
        // If there is asc, set asc
    }

    function getAscOrDescClassName(value: number) {
        return value !== -1 ? 
            value == 0 ? 
                HomeStyles.TableHeaderOrderAsc : 
                HomeStyles.TableHeaderOrderDesc
            : ""
    }
    
    return state.loading
        ? <p><em>Загрузка...</em></p>
        : <>
            <table className="table table-striped">
                <thead>
                    <tr>
                        <th className={`${HomeStyles.TableHeader} ${HomeStyles.TableHeaderOrderSelector} 
                        ${getAscOrDescClassName(orderBySelected.UserName)}`} 
                            onClick={() => setOrderBySelected({
                            UserName: changeOrderNumber(orderBySelected.UserName),
                            DateAdded: -1,
                            Email: -1
                        })}>Имя</th>
                        <th className={`${HomeStyles.TableHeader} ${HomeStyles.TableHeaderOrderSelector} 
                        ${getAscOrDescClassName(orderBySelected.Email)}`}
                            onClick={() => setOrderBySelected({
                            UserName: -1,
                            DateAdded: -1,
                            Email: changeOrderNumber(orderBySelected.Email)
                        })}>Email</th>
                        <th className={`${HomeStyles.TableHeader} ${HomeStyles.TableHeaderOrderSelector} 
                        ${getAscOrDescClassName(orderBySelected.DateAdded)}`}
                            onClick={() => setOrderBySelected({
                            UserName: -1,
                            DateAdded: changeOrderNumber(orderBySelected.DateAdded),
                            Email: -1
                        })}>Дата создания</th>
                        <th className={HomeStyles.TableHeader}>Комментарий</th>
                    </tr>
                </thead>
                <tbody>
                    {
                        state.comments.map(comment => {
                            return <tr key={comment.Id}>
                                <td>{comment.UserName}</td>
                                <td>{comment.Email}</td>
                                <td>{comment.DateAdded}</td>
                                <td><Comment comment={comment} homePageView={true}/></td>
                            </tr>
                        })
                    }
                </tbody>
            </table>
            <Pagination>
                <PaginationItem disabled={currentPage == previousPage}>
                    <PaginationLink previous onClick={() => setCurrentPage(previousPage)}/>
                </PaginationItem>
                {
                    PageChangerArray.map(page => {
                        return <PaginationItem active={currentPage == page} key={page}>
                            <PaginationLink onClick={() => setCurrentPage(page)}>
                                {page}
                            </PaginationLink>
                        </PaginationItem>
                    })   
                }
                <PaginationItem disabled={currentPage == nextPage}>
                    <PaginationLink next onClick={() => setCurrentPage(nextPage)} />
                </PaginationItem>
            </Pagination>
        </>;
}