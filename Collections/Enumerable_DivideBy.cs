
        enum DevidedItem
        {
            Exclude,
            AttachBefore,
            AttachAfter,
        }
        static IEnumerable<IEnumerable<T>> DivideBy<T>(this IEnumerable<T> source, Func<T, bool> pred, DevidedItem includeSeparator, bool RemoveEmptyEntries = true)
        {
            var queue = new Queue<T>();
            foreach(var item in source)
            {
                if (pred(item))
                {
                    switch (includeSeparator)
                    {
                        case DevidedItem.Exclude:
                        case DevidedItem.AttachAfter:
                            if (queue.Any() || !RemoveEmptyEntries)
                            {
                                yield return queue;
                                queue = new Queue<T>();
                            }
                            if(includeSeparator == DevidedItem.AttachAfter)
                                queue.Enqueue(item);
                            break;
                        case DevidedItem.AttachBefore:
                            queue.Enqueue(item);
                            yield return queue;
                            queue = new Queue<T>();
                            break;
                    }
                }
                else
                {
                    queue.Enqueue(item);
                }
            }
            if (queue.Any())
            {
                yield return queue;
            }
        }