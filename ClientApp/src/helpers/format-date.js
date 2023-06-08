import moment  from "moment";

export function formatDate(dateString) {
    // console.log(dateString);
    const newDate = moment(dateString).format('MMMM Do')
    // console.log(newDate);
    // console.log(newDate.day());
    // console.log(newDate.weekday());

    return newDate;
}