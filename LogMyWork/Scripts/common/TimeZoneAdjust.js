function dateStringToDate(val) {
    var tmp = val.split("/");
    var res = Array();
    // day
    res.push(tmp[0]);
    // month -1 (month is 0 based because why the hell not???)
    res.push((tmp[1] - 1).toString());
    tmp = tmp[2].split(" ");
    // year
    res.push(tmp[0]);
    tmp = tmp[1].split(":");
    // hour
    res.push(tmp[0]);
    // minutes
    res.push(tmp[1]);
    // seconds
    res.push(tmp[2]);
    return new Date(res[2], res[1], res[0], res[3], res[4], res[5]);
}