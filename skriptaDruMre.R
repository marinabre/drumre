install.packages('twitteR')
library(twitteR)

install.packages('tm')
library(tm)

install.packages("mongolite")
library(mongolite)

library(stringr)

install.packages("tidyr")
library(tidyr)

# #setwd('C:\Users\Ivyclover\Documents\DATA\Twitter') #postavka radnog direktorija
# setup_twitter_oauth(consumer_key = "FFpBOm40VvwEw5CglPJaFBBfE",
#                     consumer_secret = "DpZTrJLJYYTdxIGKQG5ylr2kSjypmh5v0E4y1MCjJaYbrZDix1",
#                     access_token="815934330669252609-J1AhnhWcIQ2yYotJJWANY9apdYwsTYl",
#                     access_secret="LyUxLrfGQQjnjTtQlCvs9S1yrsPvg4NAw5fzPBtQ5NNR7")
# 
# #dohvat korisnika s Twittera
# user <- getUser("@OfficialPVFC")
# #dohvat 100 tweetova od @whufc_official
# tweets <- userTimeline(user, n=100)
# tweets_text <- sapply(tweets, function(x) x$getText())
# 
# #preslikavanje tweetova u graf
# corpus <- Corpus(VectorSource(tweets_text))
# removeURL <- function(x) gsub("http[[:alnum:]]*", "", x)
# removeU <- function(x) gsub("<U[[:alnum:]]*>", "", x)
# corpus <- tm_map(corpus, removeURL)
# corpus <- tm_map(corpus, removeU)
# corpus <- tm_map(corpus, PlainTextDocument)
# corpus <- tm_map(corpus, content_transformer(tolower))
# corpus <- tm_map(corpus, content_transformer(removeNumbers))
# corpus <- tm_map(corpus, content_transformer(removePunctuation))
# corpus <- tm_map(corpus, removeWords, stopwords("english"))
# #generiranje matrice susjednosti
# tdm <- TermDocumentMatrix(corpus)
# ?tm_map
# ?Corpus
# # 
# # ?TermDocumentMatrix()
# # 
# # 
# termDocMatrix <- as.matrix(tdm)
# print(tdm)
#  termDocMatrix [termDocMatrix >=1] <- 1
#  head(termMatrix)
#  termMatrix <- termDocMatrix %*% t(termDocMatrix)
#  head(termMatrix)
#  termMatrixDF <- as.data.frame(termMatrix)
#  
#  head(termMatrixDF)
###¦¦¦¦

conection <- mongo(collection = "movies", db="projekt", url = "mongodb://ana:anaana@aws-eu-central-1-portal.0.dblayer.com:15324")

movies<- conection$find(query='{}', limit = 302, fields='{"_id":0, "Title":1, "Genres.Name":1}')
#izbjegavanje duplica
movies <- movies[!duplicated(movies$Title),]

genres <- unique(unlist(movies$Genres))
length(genres)

#pretvoriti dataframe u string odvojen razmacima
dfToList <- function(n) paste(unlist(movies$Genres[[n]]), collapse = ' ')
movies$Genres <- lapply(1:300, dfToList)

genreInList <- function(genreSpojeni) {
  rez <- ""
  for(i in 1:19){
    if( str_detect(genreSpojeni, genres[i]) )
    {  rez <- paste(rez, "1", sep = " ")
    }else{
      rez <- paste(rez, "0", sep = " ")
    }
  }
  #micanje poèetnih i krajnjih razmaka
  trimws(rez)
  }

movies$Genres <- lapply(movies$Genres, genreInList)
head(movies)

#razdioba po žanrovima, svaki stupac je jedan žanr
movies2 <- separate(movies, Genres, into=genres, sep=" ")
head(movies2)

#rezultat <- apply( <matrica>, <redovi (1) ili stupci (2)>, <funkcija> )

#diag(x) - jedinièna matrica x dimenzija
movieDataFrame <- data.frame(movies$Title, diag(length(movies$Title)))
length(unique(movies$Title))

names(movieDataFrame) <- c("Title", movies$Title)

zeros <- rep(0, length(movies$Title))

matCompare <- function(myrow){
  if(myrow[2] == 0 || myrow[2] == 1){
    n <- match(1,myrow)
    myrow2 <- ifelse(movies2[n -1,-1]== "1", TRUE, FALSE)
    rez <- zeros
    for(i in 1:length(movies$Title))
    {
        help <- ifelse(movies2[i,-1]== "1", TRUE, FALSE)
        rez[i] <- ifelse(sum(myrow2 & help) > 0, 1, 0) 
    }
    myrow <- c(myrow[1], rez)
    names(myrow) <- c("Title", movies$Title)
  }
  myrow
}

movieDataFrame <- apply(movieDataFrame, 1, matCompare)
names(movieDataFrame) <- c("Title", movies$Title)
length(movieDataFrame)

setwd("C:/Marina/FER/09DRUMRE/labos02")
write.csv2(movieDataFrame, "termMatrix.csv", quote=FALSE)
